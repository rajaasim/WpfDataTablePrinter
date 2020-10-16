# WpfDataTablePrinter
A DataGrid / DataTable Printer for WPF 

Note that all available settings have defaults.
Sample Usage

```
private void btn_print_Click(object sender, RoutedEventArgs e)
        {
            WpfDataTablePrinter wpfPrinter = new WpfDataTablePrinter();
            WpfDataTablePrinter.Settings settings = new WpfDataTablePrinter.Settings();
            List<WpfDataTablePrinter.header> headers = new List<WpfDataTablePrinter.header>();
            List<WpfDataTablePrinter.footer> footers = new List<WpfDataTablePrinter.footer>();

            WpfDataTablePrinter.header title = new WpfDataTablePrinter.header();
            title.text = "Daybook Report";
            title.fontSize = 25;
            title.bottomSpacing = 20;
            title.alignment = TextAlignment.Center;
            title.fontWeight = FontWeights.Bold;
            headers.Add(title);

            WpfDataTablePrinter.header header1 = new WpfDataTablePrinter.header();
            header1.text = "Opening Balance:3000";
            header1.fontSize = 20;
            header1.bottomSpacing = 15;
            header1.alignment = TextAlignment.Right;
            header1.fontWeight = FontWeights.Bold;
            headers.Add(header1);

            WpfDataTablePrinter.header header2 = new WpfDataTablePrinter.header();
            header2.text = "Closing Balance:5125";
            header2.fontSize = 20;
            header2.bottomSpacing = 15;
            header2.alignment = TextAlignment.Right;
            header2.fontWeight = FontWeights.Bold;
            headers.Add(header2);

            WpfDataTablePrinter.footer footer1 = new WpfDataTablePrinter.footer();
            footer1.text = "Asim";
            footer1.fontSize = 15;
            footer1.topSpacing = 10;
            footer1.alignment = TextAlignment.Left;
            footer1.fontWeight = FontWeights.Bold;
            footers.Add(footer1);

            settings.headers = headers;
            settings.footers = footers;
            settings.fontFamily = "Calibri";
            settings.columnHeadersTextAlignment = TextAlignment.Center;
            settings.fontSize = 15;
            settings.pagePadding = 50;
            settings.cellsPadding = 5;
            settings.cellsBorderThickness = 1;

            wpfPrinter.Print(daybook_table, settings);
        }
```
