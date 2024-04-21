using Microsoft.AspNetCore.Mvc;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Net.Http.Headers;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaLavanderia.Controllers
{
    public class AbastecimientoController : Controller
    {
        private readonly LavanderiaContext _context;
        private readonly INotyfService _notfy;
        public AbastecimientoController(LavanderiaContext context, INotyfService notfy)
        {
            _context = context;
            _notfy = notfy;

        }
        public async Task<IActionResult> Index()
        {
            var categoria = _context.Inventarios.Include(i => i.CategoriaNavigation);
            ViewData["Categoria"] = new SelectList(_context.CatProductos, "IdCatProducto", "DescatProducto");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReporteDeAbastecimiento(Abastecimiento abastecimiento)
        {
            QuestPDF.Settings.License = LicenseType.Community;


            if (abastecimiento.cantidadaIngresar <= 0)
            {
                _notfy.Warning("Favor de registrar cantidades en positivo");
                return View();
            }

            var cambiarCantidad = await _context.Inventarios
                .FirstOrDefaultAsync(p => p.CategoriaNavigation.IdCatProducto == abastecimiento.Categoria);

            cambiarCantidad.CantidadProducto = abastecimiento.cantidadaIngresar;
            _context.SaveChanges();


            var obtCantidad = await _context.Inventarios
                .Include(e => e.CategoriaNavigation)
                .Where((p => p.CategoriaNavigation.IdCatProducto == abastecimiento.Categoria)).ToListAsync();

            ////////////////////****************///////////////////////
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
                            col.Item().AlignCenter().Text("CCN").Bold().FontSize(20);
                            col.Item().AlignCenter().Text("Calle Luperon, al lado del Jumbo").FontSize(12);
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
                                //columns.RelativeColumn();
                                //columns.RelativeColumn();
                                //columns.RelativeColumn();
                                //columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Cantidad a Solicitar").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Prendas").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Fecha").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Estado").FontColor("#fff"); ;
                            });

                            foreach (var item in obtCantidad)
                            {
                                var producto = item.DesProductos;
                                var cantidad = item.CantidadProducto;
                                //var prendas = item.CantPrendas;
                                //var precio = item.PrecioTotal;
                                //var fecha = item.FechaPedido;
                                //var estado = item.EstadoPedidoNavigation.DesEstado;

                                tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                .Padding(2).Text(producto).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                .Padding(2).Text(cantidad.ToString()).FontSize(10);

                                //tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                //.Padding(2).Text(prendas.ToString()).FontSize(10);

                                //tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                //.Padding(2).Text(precio.ToString()).FontSize(10);

                                //tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                //.Padding(2).Text(fecha.ToString()).FontSize(10);

                                //tabla.Cell().BorderBottom(0.5f).BorderColor("D9D9D9")
                                //.Padding(2).Text(estado).FontSize(10);
                            }

                            col.Spacing(5);
                        });
                    });
                });
            }).GeneratePdf();

            Stream stream = new MemoryStream(docu);

            return File(stream, "application/pdf", "reabastecerinventario.pdf");
        }
    }
}
