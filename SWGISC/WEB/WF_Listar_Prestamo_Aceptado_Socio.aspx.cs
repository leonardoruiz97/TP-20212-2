﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;
using GestionDatos;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Data.SqlClient;
using IronPdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Image = iTextSharp.text.Image;

public partial class WF_Listar_Prestamo_Aceptado_Socio : System.Web.UI.Page
{
    Socio soci = new Socio();
    N_Socio Nso = new N_Socio();

    Prestamo pre = new Prestamo();
    N_Prestamo Npre = new N_Prestamo();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            txtCodPatrocinador.Text = Session["dni"].ToString();

            soci.IS_Dni = Convert.ToInt32(txtCodPatrocinador.Text);
            Nso.BuscarSocioxdni(soci);
            txtCodSocio.Text = "" + soci.PK_IS_Cod;
            listarprestamoaceptadosocio();



        }
    }



    void listarprestamoaceptadosocio()
    {
        soci.PK_IS_Cod = int.Parse(txtCodSocio.Text);
        gv_Tabla_Prestamo_Aceptado_Socio.DataSource = Nso.listarPrestamoAceptadoxsocio(soci);
        gv_Tabla_Prestamo_Aceptado_Socio.DataBind();
    }
    protected void gv_Tabla_Prestamo_Aceptado_Socio_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gv_Tabla_Prestamo_Aceptado_Socio.PageIndex = e.NewPageIndex;
        listarprestamoaceptadosocio();
    }

 

    protected void gv_Tabla_Prestamo_Aceptado_Socio_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string estado = DataBinder.Eval(e.Row.DataItem, "EstadoDePrestamo").ToString();
            if (estado == "Aceptado")
            {
                e.Row.Cells[8].ForeColor = System.Drawing.Color.Green;


            }
        }
    }

    protected void gv_Tabla_Prestamo_Aceptado_Socio_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Generar")//VER 

        {
            int index = Convert.ToInt32(e.CommandArgument);
            string numPrestamo = gv_Tabla_Prestamo_Aceptado_Socio.Rows[index].Cells[0].Text;
            txtNomSocioprestamoAceptada.Text = gv_Tabla_Prestamo_Aceptado_Socio.Rows[index].Cells[1].Text;
            DateTime fecha = Convert.ToDateTime(gv_Tabla_Prestamo_Aceptado_Socio.Rows[index].Cells[2].Text);
            string cuotas = gv_Tabla_Prestamo_Aceptado_Socio.Rows[index].Cells[3].Text;
            string residencia = gv_Tabla_Prestamo_Aceptado_Socio.Rows[index].Cells[4].Text;
            string importe = gv_Tabla_Prestamo_Aceptado_Socio.Rows[index].Cells[5].Text;
            string estado = gv_Tabla_Prestamo_Aceptado_Socio.Rows[index].Cells[8].Text;
            DateTime fechaInicio = fecha.AddDays(1);
            DateTime fechaExpiracion = fecha.AddDays(7);
            txtNumPrestamo.Text = numPrestamo;
            txtImportePrestamoAceptado.Text = importe;
            txtNumCuotas.Text = cuotas;
            txtFechaRegistro.Text = fecha.ToString("dd/MM/yyyy");
            txtRangoFecha.Text = fechaInicio.ToString("dd/MM/yyyy") + " " + "hasta" + " " + fechaExpiracion.ToString("dd/MM/yyyy");


            pre.PK_IPre_Cod = int.Parse(txtNumPrestamo.Text);
            pre.FK_IEPre = 4;
            Npre.ActualizarEstadoPrestamo(pre);


            tablitapdf("GENERAR COMPROBANTE PRESTAMO");
            
        }
    }

    public void tablitapdf(string strHeader)
    {
        try
        {

            //FileStream fs2 = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document pdfDoc = new Document(PageSize.A4, 15.0f, 15.0f, 30.0f, 30.0f);

            PdfWriter pdfwrite = PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);



            #region TIPOS DE FUENTE
            Font FontB = new Font(FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL));
            Font FontB8 = new Font(FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD));
            Font FontB12 = new Font(FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD));
            Font FontB16 = new Font(FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD));
            #endregion
            PdfPCell CVacio = new PdfPCell(new Phrase(""));
            CVacio.Border = 0;
            pdfDoc.Open();
            float[] a = { 4.0f, 7.0f, 1.0f, 4.0f };
            PdfPTable tabla = new PdfPTable(4);
            PdfPCell col1 = new PdfPCell();
            PdfPCell col2 = new PdfPCell();
            PdfPCell col3 = new PdfPCell();
            PdfPCell col4 = new PdfPCell();
            PdfPCell col5 = new PdfPCell();
            PdfPCell col6 = new PdfPCell();
            PdfPCell col7 = new PdfPCell();
            int ILine = 0;
            int iFila = 0;
            tabla.WidthPercentage = 95;
            tabla.SetWidths(a);
            #region Tabla1-Encabezado
            string imagenURL = Server.MapPath("/img/SanCosme.png");
            Image imagenBMP = Image.GetInstance(imagenURL);
            imagenBMP.ScaleToFit(100.0f, 100.0f);
            imagenBMP.SpacingBefore = 10.0f;
            imagenBMP.SpacingAfter = 20.0f;
            imagenBMP.SetAbsolutePosition(40, 765);
            pdfDoc.Add(imagenBMP);

            tabla.AddCell(CVacio);
            col2 = new PdfPCell(new Phrase("COOPAC SAN COSME LTDA.", FontB8));
            col2.Border = 0;
            tabla.AddCell(col2);

            tabla.AddCell(CVacio);
            col3 = new PdfPCell(new Phrase("        TICKET DE", FontB8));
            col3.Border = 0;
            tabla.AddCell(col3);

            tabla.AddCell(CVacio);
            col2 = new PdfPCell(new Phrase("Pasaje Enrique Meiggs 2123", FontB));
            col2.Border = 0;
            tabla.AddCell(col2);

            tabla.AddCell(CVacio);
            col3 = new PdfPCell(new Phrase("       DESEMBOLSO" +
                "", FontB8));
            col3.Border = 0;
            tabla.AddCell(col3);

            tabla.AddCell(CVacio);
            col2 = new PdfPCell(new Phrase("La Victoria - Lima - Perú", FontB));
            col2.Border = 0;
            tabla.AddCell(col2);

            tabla.AddCell(CVacio);
            col3 = new PdfPCell(new Phrase("RUC:10078799884", FontB8));
            col3.Border = 0;
            tabla.AddCell(col3);

            tabla.AddCell(CVacio);
            col2 = new PdfPCell(new Phrase("(993403034)", FontB));
            col2.Border = 0;
            tabla.AddCell(col2);

            tabla.AddCell(CVacio);
            col3 = new PdfPCell(new Phrase("", FontB8));
            col3.Border = 0;
            tabla.AddCell(col3);

            tabla.AddCell(CVacio);
            col2 = new PdfPCell(new Phrase("www.coopacsancosme.com", FontB));
            col2.Border = 0;
            tabla.AddCell(col2);

            tabla.AddCell(CVacio);
            col3 = new PdfPCell(new Phrase("       ", FontB8));
            col3.Border = 0;
            tabla.AddCell(col3);
            pdfDoc.Add(tabla);
            #endregion

            Paragraph p = new Paragraph(new Chunk(new LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 3)));


            pdfDoc.Add(new Paragraph("."));
            #region Tabla2-Cliente
            PdfPTable Table2 = new PdfPTable(4);
            float[] widths2 = { 2.0f, 8.0f, 3.0f, 2.0f };
            Table2.WidthPercentage = 95;
            Table2.SetWidths(widths2);
            Table2.AddCell(CVacio);
            Table2.AddCell(CVacio);
            Table2.AddCell(CVacio);
            Table2.AddCell(CVacio);
            col1 = new PdfPCell(new Phrase("Socio :", FontB8));
            col1.Border = 0;
            Table2.AddCell(col1);
            col2 = new PdfPCell(new Phrase(txtNomSocioprestamoAceptada.Text.ToUpper(), FontB));
            col2.Border = 0;
            Table2.AddCell(col2);
            col3 = new PdfPCell(new Phrase("Fecha Emisión:", FontB8));
            col3.Border = 0;
            Table2.AddCell(col3);
            col4 = new PdfPCell(new Phrase(DateTime.Today.ToShortDateString(), FontB));
            col4.Border = 0;
            Table2.AddCell(col4);

            col5 = new PdfPCell(new Phrase("Importe:", FontB8));
            col5.Border = 0;
            Table2.AddCell(col5);
            col5 = new PdfPCell(new Phrase(txtImportePrestamoAceptado.Text, FontB));
            col5.Border = 0;
            Table2.AddCell(col5);

            col1 = new PdfPCell(new Phrase("Numero de Cuotas:", FontB8));
            col1.Border = 0;
            Table2.AddCell(col1);
            col2 = new PdfPCell(new Phrase(txtNumCuotas.Text, FontB));
            col2.Border = 0;
            Table2.AddCell(col2);

            col1 = new PdfPCell(new Phrase("Distrito:", FontB8));
            col1.Border = 0;
            Table2.AddCell(col1);
            col2 = new PdfPCell(new Phrase("LIMA", FontB));
            col2.Border = 0;
            Table2.AddCell(col2);


            col3 = new PdfPCell(new Phrase("Fecha de Registro:", FontB8));
            col3.Border = 0;
            Table2.AddCell(col3);
            col4 = new PdfPCell(new Phrase(txtFechaRegistro.Text, FontB));
            col4.Border = 0;
            Table2.AddCell(col4);
            Table2.AddCell(CVacio);
            Table2.AddCell(CVacio);
            pdfDoc.Add(Table2);

            #endregion

            pdfDoc.Add(p);


            pdfDoc.Add(new Phrase("\n", FontB8));

            pdfDoc.Add(new Paragraph(12, "                                                           SU PRESTAMO HA SIDO ACEPTADO Y ESTÁ SIENDO PROCESADO.", FontB8));
            pdfDoc.Add(new Paragraph(12, "                   SIRVASE A ACERCARSE A PARTIR DE MAÑANA A NUESTRAS OFICINAS O LLÁMEMOS GRATUITAMENTE AL 993 403 034", FontB8));
            pdfDoc.Add(new Phrase("\n", FontB8));
            pdfDoc.Add(new Phrase("\n", FontB8));



            #region Tabla3-Cliente
            PdfPTable Table3 = new PdfPTable(2);
            float[] widths3 = { 2.0f, 8.0f };
            Table3.WidthPercentage = 100;
            Table3.SetWidths(widths3);
            Table3.AddCell(CVacio);
            Table3.AddCell(CVacio);
            Table3.AddCell(CVacio);
            Table3.AddCell(CVacio);
            col1 = new PdfPCell(new Phrase("Nro. de Prestamo :", FontB8));
            col1.Border = 0;
            Table3.AddCell(col1);
            col2 = new PdfPCell(new Phrase("N° " + txtNumPrestamo.Text, FontB));
            col2.Border = 0;

            pdfDoc.Add(new Phrase("", FontB8));

            Table3.AddCell(col2);
            col5 = new PdfPCell(new Phrase("Nro. de Dni :", FontB8));
            col5.Border = 0;
            Table3.AddCell(col5);
            col5 = new PdfPCell(new Phrase(txtCodPatrocinador.Text, FontB));
            col5.Border = 0;
            Table3.AddCell(col5);


            col1 = new PdfPCell(new Phrase("Agencia de Entrega :", FontB8));
            col1.Border = 0;
            Table3.AddCell(col1);



            col2 = new PdfPCell(new Phrase("Direccion : Pasaje Enrique Meiggs 2123 La Victoria - Lima - Perú", FontB));
            col2.Border = 0;
            Table3.AddCell(col2);
            col2 = new PdfPCell(new Phrase("Horario de Atención : De Lunes a Viernes de 8_30 a 17:00 hrs", FontB));
            col2.Border = 0;
            col2.PaddingLeft = 115;
            col2.Colspan = 2;
            Table3.AddCell(col2);



            col2 = new PdfPCell(new Phrase("Telefono : 0 63422918", FontB));
            col2.Border = 0;
            col2.PaddingTop = 3;
            col2.PaddingLeft = 115;
            col2.Colspan = 2;


            Table3.AddCell(col2);

            col5 = new PdfPCell(new Phrase("Fecha de Entrega Aproximada :", FontB8));
            col5.Border = 0;
            Table3.AddCell(col5);
            col5 = new PdfPCell(new Phrase(txtRangoFecha.Text, FontB));
            col5.Border = 0;
            col5.PaddingTop = 7;
            col5.Colspan = 3;
            Table3.AddCell(col5);


            col1 = new PdfPCell(new Phrase("Importante :", FontB8));
            col1.Border = 0;
            Table3.AddCell(col1);
            col2 = new PdfPCell(new Phrase("El recojo del prestamo es personal, Ud. debe llevar su DNI y Carnet de Socio. Su no tuviera el Carnet de Socio, se podra verificar en la agencia mediante un formato de Declaración Jurada indicando que es socio de la Cooperativa.", FontB));
            col2.Border = 0;
            Table3.AddCell(col2);

            col2 = new PdfPCell(new Phrase("Para Cualquier consulta Llámanos a nuestra Línea Gratuita", FontB));
            col2.Border = 0;
            col2.PaddingTop = 10;
            col2.PaddingLeft = 115;
            col2.Colspan = 2;

            Table3.AddCell(col2);


            col2 = new PdfPCell(new Phrase("942-429-202", FontB16));
            col2.Border = 0;
            col2.PaddingTop = 10;
            col2.PaddingLeft = 115;
            col2.Colspan = 2;

            Table3.AddCell(col2);



            col2 = new PdfPCell(new Phrase("Gracias por utilizar nuesto servicio", FontB16));
            col2.Border = 0;
            col2.PaddingTop = 10;
            col2.PaddingLeft = 145;
            col2.Colspan = 2;

            Table3.AddCell(col2);


            col2 = new PdfPCell(new Phrase("Todos los derechos reservados ®️ME ", FontB8));
            col2.Border = 0;
            col2.PaddingTop = 10;
            col2.PaddingLeft = 179;
            col2.Colspan = 2;

            Table3.AddCell(col2);



            col2 = new PdfPCell(new Phrase("https://www.coopacsancosme.com/", FontB8));
            col2.Border = 0;
            col2.PaddingTop = 300;
            col2.PaddingLeft = 10;
            col2.Colspan = 2;

            Table3.AddCell(col2);



            Table3.AddCell(CVacio);
            Table3.AddCell(CVacio);
            pdfDoc.Add(Table3);

            #endregion

            for (iFila = 1; iFila < 15; iFila++)
            {
                pdfDoc.Add(new Paragraph(" "));
                if (ILine < 200)
                {
                    ILine = (int)pdfwrite.GetVerticalPosition(true);
                }

            }

            pdfDoc.Close();
            pdfwrite.Close();
            //fs2.Close();

            Response.ContentType = "application/pdf";

            //Set default file Name as current datetime 
            Response.AddHeader("content-disposition", "attachment; filename=TicketPrestamo.pdf");
            System.Web.HttpContext.Current.Response.Write(pdfDoc);

            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
}