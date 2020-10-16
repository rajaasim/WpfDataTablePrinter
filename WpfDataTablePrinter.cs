using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

class WpfDataTablePrinter
{
    public class header
    {
        public string text { get; set; } = "Header";
        public double fontSize { get; set; } = 25;
        public FontWeight fontWeight { get; set; } = FontWeights.Bold;
        public TextAlignment alignment { get; set; } = TextAlignment.Center;
        public double bottomSpacing { get; set; } = 20;
        public header() { }
    }

    public class footer
    {
        public string text { get; set; } = "Footer";
        public double fontSize { get; set; } = 20;
        public FontWeight fontWeight { get; set; } = FontWeights.Bold;
        public TextAlignment alignment { get; set; } = TextAlignment.Center;
        public double topSpacing { get; set; } = 15;
        public footer() { }
    }

    public class Settings
    {
        public double cellsPadding { get; set; } = 5;
        public double cellsBorderThickness { get; set; } = 1;
        public double columnHeadersFontSize { get; set; } = 18;
        public FontWeight columnHeadersFontWeight { get; set; } = FontWeights.Bold;
        public TextAlignment columnHeadersTextAlignment { get; set; } = TextAlignment.Left;
        public double pagePadding { get; set; } = 50;
        public string fontFamily { get; set; } = "Calibri";
        public double fontSize { get; set; } = 15;
        public List<header> headers { get; set; } = new List<header>();
        public List<footer> footers { get; set; } = new List<footer>();
        public Settings() { }
    }

    public void Print(DataTable dataTable, Settings settings)
    {
        var table = new Table();

        foreach (header h in settings.headers)
        {
            var headerRowGroup = new TableRowGroup();
            table.RowGroups.Add(headerRowGroup);
            var headerRow = new TableRow();
            headerRowGroup.Rows.Add(headerRow);
            var headerCell = new TableCell(new Paragraph(new Run(h.text)));
            headerCell.ColumnSpan = dataTable.Columns.Count;
            headerCell.FontSize = h.fontSize;
            headerCell.FontFamily = new FontFamily(settings.fontFamily);
            headerCell.FontWeight = h.fontWeight;
            headerCell.TextAlignment = h.alignment;
            headerCell.Padding = new Thickness(0, 0, 0, h.bottomSpacing);
            headerRow.Cells.Add(headerCell);
        }

        var rowGroup = new TableRowGroup();
        table.RowGroups.Add(rowGroup);
        var columnsRow = new TableRow();
        rowGroup.Rows.Add(columnsRow);

        foreach (DataColumn column in dataTable.Columns)
        {
            var tableColumn = new TableColumn();
            table.Columns.Add(tableColumn);
            var cell = new TableCell(new Paragraph(new Run(column.ColumnName)));
            cell.TextAlignment = settings.columnHeadersTextAlignment;
            cell.FontSize = settings.columnHeadersFontSize;
            cell.FontWeight = settings.columnHeadersFontWeight;
            columnsRow.Cells.Add(cell);
        }

        foreach (DataRow row in dataTable.Rows)
        {
            var tableRow = new TableRow();
            rowGroup.Rows.Add(tableRow);

            foreach (DataColumn column in dataTable.Columns)
            {
                var value = row[column].ToString();
                var cell = new TableCell(new Paragraph(new Run(value)));
                cell.BorderThickness = new Thickness(settings.cellsBorderThickness);
                cell.BorderBrush = Brushes.Black;
                cell.Padding = new Thickness(settings.cellsPadding);
                tableRow.Cells.Add(cell);
            }
        }

        foreach (footer f in settings.footers)
        {
            var footerRowGroup = new TableRowGroup();
            table.RowGroups.Add(footerRowGroup);
            var footerRow = new TableRow();
            footerRowGroup.Rows.Add(footerRow);
            var footerCell = new TableCell(new Paragraph(new Run(f.text)));
            footerCell.ColumnSpan = dataTable.Columns.Count;
            footerCell.FontSize = f.fontSize;
            footerCell.FontFamily = new FontFamily(settings.fontFamily);
            footerCell.FontWeight = f.fontWeight;
            footerCell.TextAlignment = f.alignment;
            footerCell.Padding = new Thickness(0, f.topSpacing, 0, 0);
            footerRow.Cells.Add(footerCell);
        }

        table.CellSpacing = 0;
        FlowDocument document = new FlowDocument();
        document.Blocks.Add(table);
        System.IO.MemoryStream s = new System.IO.MemoryStream();
        TextRange source = new TextRange(document.ContentStart, document.ContentEnd);
        source.Save(s, DataFormats.Xaml);
        FlowDocument copy = new FlowDocument();
        TextRange dest = new TextRange(copy.ContentStart, copy.ContentEnd);
        dest.Load(s, DataFormats.Xaml);
        System.Printing.PrintDocumentImageableArea ia = null;
        System.Windows.Xps.XpsDocumentWriter docWriter = System.Printing.PrintQueue.CreateXpsDocumentWriter(ref ia);

        if (docWriter != null && ia != null)
        {
            DocumentPaginator paginator = ((IDocumentPaginatorSource)copy).DocumentPaginator;
            paginator.PageSize = new Size(ia.MediaSizeWidth, ia.MediaSizeHeight);
            Thickness t = new Thickness(settings.pagePadding);
            copy.PagePadding = new Thickness(
                             Math.Max(ia.OriginWidth, t.Left),
                               Math.Max(ia.OriginHeight, t.Top),
                               Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), t.Right),
                               Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), t.Bottom));

            copy.ColumnWidth = double.PositiveInfinity;
            copy.FontSize = settings.fontSize;
            copy.FontFamily = new FontFamily(settings.fontFamily);
            docWriter.Write(paginator);
        }

    }
}
