using Microsoft.AspNetCore.Mvc;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Net.Http.Headers;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Models;

namespace SistemaLavanderia.Controllers
{
    public class ReportesController : Controller
    {
        private readonly LavanderiaContext _context;
        private readonly INotyfService _notfy;

        public ReportesController(LavanderiaContext context, INotyfService notfy)
        {
            _context = context;
            _notfy = notfy;
        }

        public IActionResult Index()
        {
            return View();
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

            return File(stream, "application/pdf", "detalledepedidos.pdf");
        }

        public IActionResult ReporteDeConsumos()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var productos = _context.Inventarios.ToList();

            //var lavanderiaContext = _context.Pedidos
            //    .Include(p => p.ClienteNavigation)
            //    .Include(p => p.EstadoPedidoNavigation)
            //    .Include(p => p.LavadoraNavigation)
            //    .Include(p => p.ServicioNavigation).ToList();


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
                                //columns.RelativeColumn();
                                //columns.RelativeColumn();
                                //columns.RelativeColumn();
                                //columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Cantidad").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Prendas").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Fecha").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Estado").FontColor("#fff"); ;
                            });

                            foreach (var item in productos)
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

            return File(stream, "application/pdf", "detalleinventario.pdf");
        }

        public IActionResult ReporteDeFrecuenci()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var clientes = _context.Pedidos
                .GroupBy(x => x.ClienteNavigation.Nombre)
                .Select(g => new
                {
                    Nombre = g.Key,
                    Count = g.Count()
                }).ToList();

            //var lavanderiaContext = _context.Pedidos
            //    .Include(p => p.ClienteNavigation)
            //    .Include(p => p.EstadoPedidoNavigation)
            //    .Include(p => p.LavadoraNavigation)
            //    .Include(p => p.ServicioNavigation).ToList();


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
                                //columns.RelativeColumn();
                                //columns.RelativeColumn();
                                //columns.RelativeColumn();
                                //columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Cliente").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Cantidad de Freecuencia").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Prendas").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Fecha").FontColor("#fff"); ;
                                //header.Cell().Background("#257272").Padding(2).Text("Estado").FontColor("#fff"); ;
                            });

                            foreach (var item in clientes)
                            {
                                var producto = item.Nombre;
                                var cantidad = item.Count;
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

            return File(stream, "application/pdf", "detalleclientes.pdf");
        }
    }
}
