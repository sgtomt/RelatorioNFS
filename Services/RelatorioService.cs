using ClosedXML.Excel;
using Microsoft.AspNetCore.Components.Forms;
using nf_app_v2.Models;
using System.Globalization;
using System.Xml.Linq;

namespace nf_app_v2.Services
{
    public class RelatorioService
    {
        public async Task<byte[]> GerarRelatorioAsync(List<IBrowserFile> arquivos)
        {
            List<RegistroXml> registros = new List<RegistroXml>();
            foreach (var arquivo in arquivos)
            {
                var registro = await ObterDadosXml(arquivo);
                registros.Add(registro);            
            }

            return GerarExcel(registros);
        }

        private async Task<RegistroXml> ObterDadosXml(IBrowserFile arquivo)
        {
            // 1. Defina um limite maior (ex: 10MB)
            // 2. Copie para um MemoryStream IMEDIATAMENTE para não perder a conexão com o navegador
            using var stream = arquivo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0; // Volta para o início para o XDocument ler

            var doc = await XDocument.LoadAsync(ms, System.Xml.Linq.LoadOptions.None, CancellationToken.None);
            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

            var emit = doc.Descendants(ns + "emit").FirstOrDefault();
            var ide = doc.Descendants(ns + "ide").FirstOrDefault();
            var cobr = doc.Descendants(ns + "cobr").FirstOrDefault();

            var nome = emit?.Element(ns + "xNome")?.Value ?? "N/A";
            var nomeFantasia = emit?.Element(ns + "xFant")?.Value ?? "N/A";
            var cnpj = emit?.Element(ns + "CNPJ")?.Value ?? "N/A";

            var fat = cobr?.Element(ns + "fat");
            var dhEmi = ide?.Element(ns + "dhEmi")?.Value ?? "N/A";
            var nFat = fat?.Element(ns + "nFat")?.Value ?? "N/A";
            var vOrig = fat?.Element(ns + "vOrig")?.Value ?? "N/A";

            var registro = new RegistroXml
            {
                NomeArquivo = arquivo.Name,
                NumeroNota = nFat,
                DataEmissao = dhEmi,
                RazaoSocial = nome,
                NomeFantasia = nomeFantasia,
                CNPJEmitente = cnpj,
                ValorNota = vOrig
            };

            return registro;
        }

        private byte[] GerarExcel(List<RegistroXml> dados)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Relatório NF");

            //Cabeçalho
            ws.Cell(1, 1).Value = "NumeroNota";
            ws.Cell(1, 2).Value = "ValorNota";
            ws.Cell(1, 3).Value = "DataEmissao";
            ws.Cell(1, 4).Value = "RazaoSocial";
            ws.Cell(1, 5).Value = "NomeFantasia";
            ws.Cell(1, 6).Value = "CNPJEmitente";
            ws.Cell(1, 7).Value = "Arquivo";

            var header = ws.Range(1, 1, 1, 7);

            // 🎨 Estilo do cabeçalho
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.AzureColorWheel;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int linha = 2;

            foreach (var item in dados)
            {
                ws.Cell(linha, 1).Value = item.NumeroNota;

                if (decimal.TryParse(item.ValorNota,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var valor))
                {
                    ws.Cell(linha, 2).Value = valor;
                    ws.Cell(linha, 2).Style.NumberFormat.Format = "R$ #,##0.00";
                }

                // 📅 Data formatada
                if (DateTime.TryParse(item.DataEmissao, out DateTime data))
                {
                    ws.Cell(linha, 3).Value = data;
                    ws.Cell(linha, 3).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                }

                ws.Cell(linha, 4).Value = item.RazaoSocial;
                ws.Cell(linha, 5).Value = item.NomeFantasia;
                ws.Cell(linha, 6).Value = item.CNPJEmitente;
                ws.Cell(linha, 7).Value = item.NomeArquivo;
                linha++;
            }

            // 📏 Ajusta largura automaticamente
            ws.Columns().AdjustToContents();
            ws.Column(6).Style.Alignment.WrapText = true;
            ws.SheetView.FreezeRows(1);

            // 🔲 Bordas na tabela
            var range = ws.Range(1, 1, linha - 1, 7);
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // 📊 Transforma em tabela (fica bonito no Excel)
            range.CreateTable();
            ws.Columns().AdjustToContents();
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }
    }
}
