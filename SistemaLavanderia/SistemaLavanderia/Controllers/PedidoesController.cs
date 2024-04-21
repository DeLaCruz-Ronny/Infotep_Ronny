using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Models;


using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Net.Http.Headers;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace SistemaLavanderia.Controllers
{
    public class PedidoesController : Controller
    {
        private readonly LavanderiaContext _context;
        private readonly INotyfService _notfy;

        public PedidoesController(LavanderiaContext context, INotyfService notfy)
        {
            _context = context;
            _notfy = notfy;
        }

        // GET: Pedidoes
        public async Task<IActionResult> Index()
        {
            var lavanderiaContext = _context.Pedidos.Include(p => p.ClienteNavigation).Include(p => p.EstadoPedidoNavigation).Include(p => p.LavadoraNavigation).Include(p => p.ServicioNavigation);
            return View(await lavanderiaContext.ToListAsync());
        }

        // GET: Pedidoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.ClienteNavigation)
                .Include(p => p.EstadoPedidoNavigation)
                .Include(p => p.LavadoraNavigation)
                .Include(p => p.ServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidoes/Create
        public IActionResult Create()
        {
            
            ViewData["Cliente"] = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            ViewData["EstadoPedido"] = new SelectList(_context.Estados, "IdEstado", "DesEstado");
            ViewData["Lavadora"] = new SelectList(_context.Lavadoras, "IdLavadoras", "DesLavadoras");
            ViewData["Servicio"] = new SelectList(_context.Servicios, "IdServicio", "DesServicio");
            return View();
        }

        // POST: Pedidoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cliente,Servicio,Lavadora,CantPrendas,PrecioTotal,FechaPedido,EstadoPedido,IdPedido")] Pedido pedido)
        {
            var disponibilidadEnelInventario = _context.Inventarios
                .Where(c => c.Servicio == pedido.Servicio)
                .Select(a => a.CantidadProducto)
                .FirstOrDefault();

            if (disponibilidadEnelInventario == 0)
            {
                //ViewData["NoDisponibilidad"] = "El servicio seleccionado no tiene disponibilidad en el inventario";
                _notfy.Error("El servicio seleccionado no tiene disponibilidad en el inventario",3);
                return View();
            }

            var lava = _context.Lavadoras.FirstOrDefaultAsync(x => x.Disponible.Value);
            if (lava.Result == null)
            {
                pedido.EstadoPedido = 4;

                if (disponibilidadEnelInventario <= 5 && disponibilidadEnelInventario >= 1)
                {
                    //ViewData["PocaDisponibilidad"] = "El servicio seleccionado tiene poca disponibilidad en el inventario";
                    _notfy.Information("El servicio seleccionado tiene poca disponibilidad en el inventario",3);
                    var reducir = await _context.Inventarios.FirstOrDefaultAsync(l => l.Servicio == pedido.Servicio);
                    reducir.CantidadProducto = disponibilidadEnelInventario  - 1;
                }

                if (ModelState.IsValid)
                {
                    _context.Add(pedido);
                    await _context.SaveChangesAsync();
                    _notfy.Success("Registro creado, ha sido puesto en truno");
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                pedido.EstadoPedido = 1;
                pedido.Lavadora = _context.Lavadoras.Where(p => p.Disponible == true).Select(r => r.IdLavadoras).FirstOrDefault();
                //Obtener la lavadora disponible
                var disp = _context.Lavadoras.Where(p => p.Disponible == true).Select(r => r.IdLavadoras).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    _context.Add(pedido);

                    if (disponibilidadEnelInventario <= 5 && disponibilidadEnelInventario >= 1)
                    {
                        //ViewData["PocaDisponibilidad"] = "El servicio seleccionado tiene poca disponibilidad en el inventario";
                        _notfy.Information("El servicio seleccionado tiene poca disponibilidad en el inventario");
                        var reducir = await _context.Inventarios.FirstOrDefaultAsync(l => l.Servicio == disponibilidadEnelInventario);
                        reducir.CantidadProducto = disponibilidadEnelInventario - 1;
                    }

                    var nuevoEstado = await _context.Lavadoras.FirstOrDefaultAsync(l => l.IdLavadoras == disp);
                    if (nuevoEstado != null)
                    {
                        // Actualiza las propiedades de la lavadora
                        nuevoEstado.Disponible = false;

                        //// Guarda los cambios en la base de datos
                        //await _context.SaveChangesAsync();
                    }

                    await _context.SaveChangesAsync();
                    _notfy.Success("Pedido Creado",3);
                    return RedirectToAction(nameof(Index));
                }

            }

            
            ViewData["Cliente"] = new SelectList(_context.Clientes, "IdCliente", "Nombre", pedido.Cliente);
            ViewData["EstadoPedido"] = new SelectList(_context.Estados, "IdEstado", "DesEstado", pedido.EstadoPedido);
            ViewData["Lavadora"] = new SelectList(_context.Lavadoras, "IdLavadoras", "DesLavadoras", pedido.Lavadora);
            ViewData["Servicio"] = new SelectList(_context.Servicios, "IdServicio", "DesServicio", pedido.Servicio);
            return View(pedido);
        }

        // GET: Pedidoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["Cliente"] = new SelectList(_context.Clientes, "IdCliente", "Nombre", pedido.Cliente);
            ViewData["EstadoPedido"] = new SelectList(_context.Estados, "IdEstado", "DesEstado", pedido.EstadoPedido);
            ViewData["Lavadora"] = new SelectList(_context.Lavadoras, "IdLavadoras", "DesLavadoras", pedido.Lavadora);
            ViewData["Servicio"] = new SelectList(_context.Servicios, "IdServicio", "DesServicio", pedido.Servicio);
            return View(pedido);
        }

        // POST: Pedidoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cliente,Servicio,Lavadora,CantPrendas,PrecioTotal,FechaPedido,EstadoPedido,IdPedido")] Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.IdPedido))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cliente"] = new SelectList(_context.Clientes, "IdCliente", "Nombre", pedido.Cliente);
            ViewData["EstadoPedido"] = new SelectList(_context.Estados, "IdEstado", "DesEstado", pedido.EstadoPedido);
            ViewData["Lavadora"] = new SelectList(_context.Lavadoras, "IdLavadoras", "DesLavadoras", pedido.Lavadora);
            ViewData["Servicio"] = new SelectList(_context.Servicios, "IdServicio", "DesServicio", pedido.Servicio);
            return View(pedido);
        }

        // GET: Pedidoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.ClienteNavigation)
                .Include(p => p.EstadoPedidoNavigation)
                .Include(p => p.LavadoraNavigation)
                .Include(p => p.ServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }


        public IActionResult ReportePedidos()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var pedidos = _context.Pedidos.ToList();

            var lavanderiaContext = _context.Pedidos
                .Include(p => p.ClienteNavigation)
                .Include(p => p.EstadoPedidoNavigation)
                .Include(p => p.LavadoraNavigation)
                .Include(p => p.ServicioNavigation).ToList();


            //Se crea el documento
            var docu = Document.Create(documento =>
            {
                documento.Page(page =>
                {
                    page.Margin(30);

                    page.Header().ShowOnce().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Lavanderia el Buen Pastor").Bold().FontSize(20);
                            col.Item().AlignCenter().Text("Calle Garcia godoy #5, Bajos de Haina").FontSize(12);
                            col.Item().AlignCenter().Text("Tel: 000-000-0000").FontSize(12);
                            col.Item().AlignCenter().Text("Email: correo@gmail.com").FontSize(12);
                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Cliente").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Servicio").FontColor("#fff"); ;
                                header.Cell().Background("#257272").Padding(2).Text("Prendas").FontColor("#fff"); ;
                                header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff"); ;
                                header.Cell().Background("#257272").Padding(2).Text("Fecha").FontColor("#fff"); ;
                                header.Cell().Background("#257272").Padding(2).Text("Estado").FontColor("#fff"); ;
                            });

                            foreach (var item in lavanderiaContext)
                            {
                                var cliente = item.ClienteNavigation.Nombre;
                                var servicio = item.ServicioNavigation.DesServicio;
                                var prendas = item.CantPrendas;
                                var precio = item.PrecioTotal;
                                var fecha = item.FechaPedido;
                                var estado = item.EstadoPedidoNavigation.DesEstado;

                                tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                .Padding(2).Text(cliente).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                .Padding(2).Text(servicio).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                .Padding(2).Text(prendas.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                .Padding(2).Text(precio.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                .Padding(2).Text(fecha.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                .Padding(2).Text(estado).FontSize(10);
                            }

                            col.Spacing(5);
                        });
                    });
                });
            }).GeneratePdf();

            Stream stream = new MemoryStream(docu);

            return File(stream, "application/pdf","detalledepedidos.pdf");
        }


        public async Task<IActionResult> NotificacionPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return View();
            }

            //Token
            string token = "EAAHBsBGNLNABOzLpqjNwb5rSg0CeLIjs1uWSN8kLTeG3nm3xMaLBP3fd9q8PZA27s4DZCUgqfRdjDfk7gjEArO56Idp0xZBbHoIlq2ED9rfVomTRozpqxGU490DxaovZAxxe25ipgGHwgWbEpaubq5dAcwK3OUsVeFPwLyWZARobFAb6R5B8dCtPl39jsMSXZApNOdpPmlzu3cmu28";
            //Identificador de número de teléfono
            string idTelefono = "291141714081979";
            //Nuestro telefono
            string telefono = "18496244560";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v15.0/" + idTelefono + "/messages");
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Content = new StringContent("{ \"messaging_product\": \"whatsapp\", \"to\": \"" + telefono + "\", \"type\": \"template\", \"template\": { \"name\": \"hello_world\", \"language\": { \"code\": \"en_US\" } } }");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index", "Pedidoes");

        }
    }
}
