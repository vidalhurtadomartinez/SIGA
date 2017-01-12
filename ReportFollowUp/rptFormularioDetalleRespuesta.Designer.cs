namespace ReporteOYM
{
    partial class rptFormularioDetalleRespuesta
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rptFormularioDetalleRespuesta));
            Telerik.Reporting.NumericalScale numericalScale1 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.groupFooterSection1 = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection1 = new Telerik.Reporting.GroupHeaderSection();
            this.panel1 = new Telerik.Reporting.Panel();
            this.textBox24 = new Telerik.Reporting.TextBox();
            this.textBox25 = new Telerik.Reporting.TextBox();
            this.textBox26 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox21 = new Telerik.Reporting.TextBox();
            this.sqlDataSourceTipoProceso = new Telerik.Reporting.SqlDataSource();
            this.sqlDataSourceFormulario = new Telerik.Reporting.SqlDataSource();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.panel2 = new Telerik.Reporting.Panel();
            this.textBox22 = new Telerik.Reporting.TextBox();
            this.textBox23 = new Telerik.Reporting.TextBox();
            this.textBox27 = new Telerik.Reporting.TextBox();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.sqlDataSourceLista = new Telerik.Reporting.SqlDataSource();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.graphAxis6 = new Telerik.Reporting.GraphAxis();
            this.sqlDataSourceDetalleRespuesta = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.30000066757202148D);
            this.groupFooterSection.Name = "groupFooterSection";
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.80000007152557373D);
            this.groupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1});
            this.groupHeaderSection.Name = "groupHeaderSection";
            this.groupHeaderSection.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.groupHeaderSection.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.groupHeaderSection.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.groupHeaderSection.Style.Font.Bold = true;
            this.groupHeaderSection.Style.LineColor = System.Drawing.Color.LightGray;
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.899900436401367D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox1.Value = "=\"TIPO PROCESO : \" + Fields.TipoProcesoDesc + \" (\" + Count(Fields.lTipoProceso_id" +
    ") + \")\"";
            // 
            // groupFooterSection1
            // 
            this.groupFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.29979842901229858D);
            this.groupFooterSection1.Name = "groupFooterSection1";
            // 
            // groupHeaderSection1
            // 
            this.groupHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.8000003099441528D);
            this.groupHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel1,
            this.textBox2,
            this.textBox6,
            this.textBox14,
            this.textBox15,
            this.textBox16,
            this.textBox17,
            this.textBox3,
            this.textBox9,
            this.textBox21});
            this.groupHeaderSection1.Name = "groupHeaderSection1";
            this.groupHeaderSection1.Style.BorderColor.Default = System.Drawing.Color.LightGray;
            this.groupHeaderSection1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox24,
            this.textBox25,
            this.textBox26});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.9000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(0.90000033378601074D));
            this.panel1.Style.BorderColor.Default = System.Drawing.Color.DarkGray;
            this.panel1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox24
            // 
            this.textBox24.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000030100345612D), Telerik.Reporting.Drawing.Unit.Cm(0.10000035166740418D));
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.9048082828521729D), Telerik.Reporting.Drawing.Unit.Cm(0.60312491655349731D));
            this.textBox24.Style.Font.Bold = true;
            this.textBox24.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox24.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox24.Value = "FECHA DE RESPUESTA";
            // 
            // textBox25
            // 
            this.textBox25.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.1102175712585449D), Telerik.Reporting.Drawing.Unit.Cm(0.10000035166740418D));
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.9897823333740234D), Telerik.Reporting.Drawing.Unit.Cm(0.60312491655349731D));
            this.textBox25.Style.Font.Bold = true;
            this.textBox25.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox25.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox25.Value = "CATEGORIA";
            // 
            // textBox26
            // 
            this.textBox26.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.1001996994018555D), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941D));
            this.textBox26.Name = "textBox26";
            this.textBox26.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.8998012542724609D), Telerik.Reporting.Drawing.Unit.Cm(0.60312491655349731D));
            this.textBox26.Style.Font.Bold = true;
            this.textBox26.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox26.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox26.Value = "COMENTARIO";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0052088228985667229D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1048083305358887D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.Value = "= Fields.Titulo";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.4050087928771973D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3997993469238281D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.Value = "= Fields.TiempoElaboracion";
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.8050088882446289D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.4999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox14.Value = "= Fields.ElaboradoPorDesc";
            // 
            // textBox15
            // 
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.105008125305176D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.4999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox15.Value = "= Fields.RevisadoPorDesc";
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(20.705007553100586D), Telerik.Reporting.Drawing.Unit.Cm(9.9315642728470266E-05D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.4999997615814209D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox16.Value = "= Fields.AprobadoPorDesc";
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.205207824707031D), Telerik.Reporting.Drawing.Unit.Cm(9.9315642728470266E-05D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4145975112915039D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox17.Value = "= Fields.TiempoTotal";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.1102170944213867D), Telerik.Reporting.Drawing.Unit.Cm(9.9315642728470266E-05D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.294590950012207D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "= Fields.sCodigo";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.305209159851074D), Telerik.Reporting.Drawing.Unit.Cm(9.9315642728470266E-05D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7995995283126831D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox9.Value = "= Fields.TiempoRevision";
            // 
            // textBox21
            // 
            this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.60521125793457D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0996005535125732D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox21.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox21.Value = "= Fields.TiempoAprobacion";
            // 
            // sqlDataSourceTipoProceso
            // 
            this.sqlDataSourceTipoProceso.ConnectionString = "SeguimientoReports.Properties.Settings.DBSeguimiento";
            this.sqlDataSourceTipoProceso.Name = "sqlDataSourceTipoProceso";
            this.sqlDataSourceTipoProceso.SelectCommand = "Select * From tblTipoProceso where bactivo = 1";
            // 
            // sqlDataSourceFormulario
            // 
            this.sqlDataSourceFormulario.ConnectionString = "SeguimientoReports.Properties.Settings.DBSeguimiento";
            this.sqlDataSourceFormulario.Name = "sqlDataSourceFormulario";
            this.sqlDataSourceFormulario.SelectCommand = "Select * From tblFormulario";
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.1000001430511475D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pictureBox1,
            this.textBox19});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0052088228985667229D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.pictureBox1.MimeType = "image/png";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(2.0000002384185791D));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // textBox19
            // 
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.90000057220459D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(21.719902038574219D), Telerik.Reporting.Drawing.Unit.Cm(2D));
            this.textBox19.Style.Font.Bold = true;
            this.textBox19.Style.Font.Name = "Segoe UI";
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox19.Value = "DETALLE DE RESPUESTAS POR PROCESOS DE FORMULARIOS";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.999999463558197D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel2});
            this.detail.Name = "detail";
            this.detail.Style.BorderColor.Default = System.Drawing.Color.LightGray;
            this.detail.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.detail.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.detail.Style.Color = System.Drawing.Color.Black;
            this.detail.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.detail.Style.LineColor = System.Drawing.Color.LightGray;
            this.detail.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            // 
            // panel2
            // 
            this.panel2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox22,
            this.textBox23,
            this.textBox27});
            this.panel2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.9000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.panel2.Name = "panel2";
            this.panel2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(0.79999959468841553D));
            this.panel2.Style.BorderColor.Default = System.Drawing.Color.LightGray;
            this.panel2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox22
            // 
            this.textBox22.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.1102175712585449D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox22.Name = "textBox22";
            this.textBox22.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.9897820949554443D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox22.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox22.Value = "= Fields.CategoriaDesc";
            // 
            // textBox23
            // 
            this.textBox23.Format = "{0:G}";
            this.textBox23.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.2000001072883606D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.904808521270752D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox23.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox23.Value = "= Fields.dtFechaRespuesta_dt";
            // 
            // textBox27
            // 
            this.textBox27.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.1001996994018555D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.8998012542724609D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.textBox27.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox27.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox27.Value = "= Fields.ComentarioRespuesta";
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox20});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // textBox20
            // 
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4D), Telerik.Reporting.Drawing.Unit.Cm(0.60000020265579224D));
            this.textBox20.Style.Font.Name = "Segoe UI";
            this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox20.Value = "Pag.: {PageNumber}";
            // 
            // sqlDataSourceLista
            // 
            this.sqlDataSourceLista.ConnectionString = "SeguimientoReports.Properties.Settings.DBSeguimiento";
            this.sqlDataSourceLista.Name = "sqlDataSourceLista";
            this.sqlDataSourceLista.SelectCommand = "dbo.sp_FormularioDetalleRespuestaPorProcesoReporte";
            this.sqlDataSourceLista.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.0999999046325684D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox4,
            this.textBox5,
            this.textBox8,
            this.textBox11,
            this.textBox12,
            this.textBox13,
            this.textBox10,
            this.textBox7,
            this.textBox18});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            this.reportHeaderSection1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Double;
            this.reportHeaderSection1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Double;
            this.reportHeaderSection1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Double;
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0052088228985667229D), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0995998382568359D), Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209D));
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.Value = "TITULO DEL DOCUMENTO";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.105008602142334D), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.2945911884307861D), Telerik.Reporting.Drawing.Unit.Cm(0.90000033378601074D));
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.Value = "CODIGO";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.3997998237609863D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3997993469238281D), Telerik.Reporting.Drawing.Unit.Cm(0.89416652917861938D));
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.Value = "TIEMPO ELABORACION";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.099800109863281D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5D), Telerik.Reporting.Drawing.Unit.Cm(0.89416652917861938D));
            this.textBox11.Style.Font.Bold = true;
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox11.Value = "REVISADO POR";
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(20.699800491333008D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.4999995231628418D), Telerik.Reporting.Drawing.Unit.Cm(0.89416652917861938D));
            this.textBox12.Style.Font.Bold = true;
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox12.Value = "APROBADO POR";
            // 
            // textBox13
            // 
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(24.200000762939453D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4199023246765137D), Telerik.Reporting.Drawing.Unit.Cm(0.89416652917861938D));
            this.textBox13.Style.Font.Bold = true;
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox13.Value = "TOTAL TIEMPO";
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.799799919128418D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5D), Telerik.Reporting.Drawing.Unit.Cm(0.89416652917861938D));
            this.textBox10.Style.Font.Bold = true;
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.Value = "ELABORADOR POR";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.300000190734863D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7995994091033936D), Telerik.Reporting.Drawing.Unit.Cm(0.89416652917861938D));
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.Value = "TIEMPO REVISION";
            // 
            // textBox18
            // 
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.600000381469727D), Telerik.Reporting.Drawing.Unit.Cm(0.099999547004699707D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0996005535125732D), Telerik.Reporting.Drawing.Unit.Cm(0.89416652917861938D));
            this.textBox18.Style.Font.Bold = true;
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox18.Value = "TIEMPO APROB.";
            // 
            // graphAxis6
            // 
            this.graphAxis6.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis6.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis6.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis6.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis6.MinorGridLineStyle.Visible = false;
            this.graphAxis6.Name = "GraphAxis2";
            this.graphAxis6.Scale = numericalScale1;
            // 
            // sqlDataSourceDetalleRespuesta
            // 
            this.sqlDataSourceDetalleRespuesta.ConnectionString = "SeguimientoReports.Properties.Settings.DBSeguimiento";
            this.sqlDataSourceDetalleRespuesta.Name = "sqlDataSourceDetalleRespuesta";
            this.sqlDataSourceDetalleRespuesta.SelectCommand = "dbo.sp_DocumentoDetalleRespuestaReporte";
            this.sqlDataSourceDetalleRespuesta.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // rptFormularioDetalleRespuesta
            // 
            this.BookmarkId = "";
            this.Culture = new System.Globalization.CultureInfo("");
            this.DataSource = this.sqlDataSourceLista;
            this.DocumentName = "Detalle Respuesta";
            this.Filters.Add(new Telerik.Reporting.Filter("= Fields.lTipoProceso_id", Telerik.Reporting.FilterOperator.In, "= Parameters.TipoProceso.Value"));
            this.Filters.Add(new Telerik.Reporting.Filter("= Fields.Id", Telerik.Reporting.FilterOperator.In, "= Parameters.Formulario.Value"));
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.lTipoProceso_id"));
            group1.Name = "group";
            group2.BookmarkId = "= Fields.Titulo";
            group2.DocumentMapText = "= Fields.Titulo";
            group2.GroupFooter = this.groupFooterSection1;
            group2.GroupHeader = this.groupHeaderSection1;
            group2.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.lFormularioProceso_id"));
            group2.Name = "group1";
            group2.Sortings.Add(new Telerik.Reporting.Sorting("= Fields.lFormularioProceso_id", Telerik.Reporting.SortDirection.Asc));
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.groupHeaderSection1,
            this.groupFooterSection1,
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1,
            this.reportHeaderSection1});
            this.Name = "rptFormularioDetalleRespuesta";
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(15.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(15.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(15.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(15.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AllowNull = true;
            reportParameter1.AvailableValues.DataSource = this.sqlDataSourceTipoProceso;
            reportParameter1.AvailableValues.DisplayMember = "= Fields.sDescripcion";
            reportParameter1.AvailableValues.ValueMember = "= Fields.lTipoProceso_id";
            reportParameter1.MultiValue = true;
            reportParameter1.Name = "TipoProceso";
            reportParameter1.Text = "Tipo Proceso";
            reportParameter1.Visible = true;
            reportParameter2.AllowNull = true;
            reportParameter2.AvailableValues.DataSource = this.sqlDataSourceFormulario;
            reportParameter2.AvailableValues.DisplayMember = "= Fields.Titulo";
            reportParameter2.AvailableValues.ValueMember = "= Fields.Id";
            reportParameter2.MultiValue = true;
            reportParameter2.Name = "Formulario";
            reportParameter2.Text = "Formulario";
            reportParameter2.Visible = true;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.Style.Font.Name = "Segoe UI";
            this.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(26.619903564453125D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.SqlDataSource sqlDataSourceLista;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox20;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.GraphAxis graphAxis6;
        private Telerik.Reporting.SqlDataSource sqlDataSourceTipoProceso;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox21;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.SqlDataSource sqlDataSourceDetalleRespuesta;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection1;
        private Telerik.Reporting.GroupFooterSection groupFooterSection1;
        private Telerik.Reporting.TextBox textBox22;
        private Telerik.Reporting.TextBox textBox23;
        private Telerik.Reporting.TextBox textBox27;
        private Telerik.Reporting.TextBox textBox24;
        private Telerik.Reporting.TextBox textBox25;
        private Telerik.Reporting.TextBox textBox26;
        private Telerik.Reporting.Panel panel1;
        private Telerik.Reporting.Panel panel2;
        private Telerik.Reporting.SqlDataSource sqlDataSourceFormulario;
    }
}