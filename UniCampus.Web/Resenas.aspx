
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Resenas.aspx.cs"
    Inherits="UniCampus.Web.Resenas" %>

<!DOCTYPE html>
<html lang="es">

<head runat="server">

    <meta charset="utf-8" />

    <title>UniCampus - Reseñas</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f6f9;
            margin: 0;
            padding: 30px;
        }

        .contenedor {
            max-width: 1100px;
            margin: auto;
            background-color: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 2px 10px #cccccc;
        }

        /* MENU DE NAVEGACION */
        .menu {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            background-color: #203864;
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 25px;
        }

        .menu a {
            color: white;
            text-decoration: none;
            padding: 11px 18px;
            border-radius: 5px;
            font-weight: bold;
        }

        .menu a:hover:not(.activo) {
            background-color: #365b91;
        }

        .menu .activo {
            background-color: #2874a6;
        }

        h1 {
            color: #234a80;
            text-align: center;
        }

        h2 {
            color: #333333;
        }

        .campo {
            margin-bottom: 15px;
        }

        .campo label {
            display: block;
            font-weight: bold;
            margin-bottom: 6px;
        }

        .entrada {
            width: 100%;
            max-width: 450px;
            padding: 10px;
            border: 1px solid #cccccc;
            border-radius: 5px;
            box-sizing: border-box;
            font-family: Arial, sans-serif;
        }

        .entrada:disabled {
            background-color: #eeeeee;
            color: #555555;
        }

        /* BOTONES */
        .boton {
            background-color: #234a80;
            color: white;
            padding: 12px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            margin-right: 8px;
            margin-bottom: 8px;
        }

        .boton:hover {
            background-color: #16345d;
        }

        .botonModificar {
            background-color: #e09a20;
        }

        .botonModificar:hover {
            background-color: #bd7b10;
        }

        .botonCancelar {
            background-color: #777777;
        }

        .botonCancelar:hover {
            background-color: #555555;
        }

        .botonTabla {
            display: inline-block;
            padding: 8px 12px;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            text-decoration: none;
            font-size: 13px;
            margin: 3px;
        }

        .botonEditarTabla {
            background-color: #e09a20;
        }

        .botonEditarTabla:hover {
            background-color: #bd7b10;
        }

        .botonEliminarTabla {
            background-color: #c0392b;
        }

        .botonEliminarTabla:hover {
            background-color: #922b21;
        }

        .mensaje {
            display: block;
            margin-top: 15px;
            margin-bottom: 15px;
            font-weight: bold;
            color: #234a80;
        }

        /* TABLA */
        .tablaContenedor {
            width: 100%;
            overflow-x: auto;
        }

        .tabla {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }

        .tabla th {
            background-color: #234a80;
            color: white;
            padding: 12px;
            text-align: center;
        }

        .tabla td {
            padding: 10px;
            border-bottom: 1px solid #dddddd;
            text-align: center;
        }

        .tabla tr:hover td {
            background-color: #f4f6f9;
        }

        .acciones {
            white-space: nowrap;
        }

        hr {
            border: none;
            border-top: 1px solid #dddddd;
            margin: 25px 0;
        }

        @media (max-width: 650px) {
            body {
                padding: 10px;
            }

            .contenedor {
                padding: 15px;
            }

            .menu {
                flex-direction: column;
            }

            .menu a {
                text-align: center;
            }
        }
    </style>

</head>

<body>

    <form id="form1" runat="server">

        <div class="contenedor">

            <!-- MENU PRINCIPAL -->
            <nav class="menu">

                <a href="Default.aspx">
                    Inicio
                </a>

                <a href="Actividades.aspx">
                    Actividades
                </a>

                <a href="Inscripciones.aspx">
                    Inscripciones
                </a>

                <a href="Resenas.aspx"
                   class="activo"
                   aria-current="page">
                    Reseñas
                </a>

                <a href="Estadisticas.aspx">
                    Estadísticas
                </a>

                <!-- SOLO LOS DOCENTES VEN ESTUDIANTES -->
                <% if (Convert.ToString(Session["RolUsuario"]) == "Organizador") { %>

                    <a href="Estudiantes.aspx">
                        Estudiantes
                    </a>

                <% } %>

            </nav>

            <h1>UniCampus</h1>

            <h2>Registro y administración de reseñas</h2>

            <!-- ACTIVIDAD ACADEMICA -->
            <div class="campo">

                <label>Actividad académica</label>

                <asp:DropDownList
                    ID="ddlActividad"
                    runat="server"
                    CssClass="entrada"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlActividad_SelectedIndexChanged">
                </asp:DropDownList>

            </div>

            <!-- ESTUDIANTE -->
            <div class="campo">

                <label>Nombre del estudiante</label>

                <asp:DropDownList
                    ID="ddlEstudiante"
                    runat="server"
                    CssClass="entrada">
                </asp:DropDownList>

            </div>

            <!-- CALIFICACION -->
            <div class="campo">

                <label>Calificación</label>

                <asp:DropDownList
                    ID="ddlCalificacion"
                    runat="server"
                    CssClass="entrada">

                    <asp:ListItem
                        Text="-- Seleccione una calificación --"
                        Value="0" />

                    <asp:ListItem
                        Text="1 - Muy mala"
                        Value="1" />

                    <asp:ListItem
                        Text="2 - Mala"
                        Value="2" />

                    <asp:ListItem
                        Text="3 - Regular"
                        Value="3" />

                    <asp:ListItem
                        Text="4 - Buena"
                        Value="4" />

                    <asp:ListItem
                        Text="5 - Excelente"
                        Value="5" />

                </asp:DropDownList>

            </div>

            <!-- COMENTARIO -->
            <div class="campo">

                <label>Comentario</label>

                <asp:TextBox
                    ID="txtComentario"
                    runat="server"
                    TextMode="MultiLine"
                    Rows="4"
                    CssClass="entrada">
                </asp:TextBox>

            </div>

            <!-- BOTON REGISTRAR -->
            <asp:Button
                ID="btnGuardarResena"
                runat="server"
                Text="Registrar reseña"
                CssClass="boton"
                OnClick="btnGuardarResena_Click" />

            <!-- BOTON GUARDAR MODIFICACION -->
            <asp:Button
                ID="btnModificarResena"
                runat="server"
                Text="Guardar cambios"
                CssClass="boton botonModificar"
                Visible="false"
                OnClick="btnModificarResena_Click" />

            <!-- BOTON CANCELAR EDICION -->
            <asp:Button
                ID="btnCancelarEdicion"
                runat="server"
                Text="Cancelar edición"
                CssClass="boton botonCancelar"
                Visible="false"
                CausesValidation="false"
                OnClick="btnCancelarEdicion_Click" />

            <!-- MENSAJES -->
            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="mensaje">
            </asp:Label>

            <hr />

            <!-- TABLA DE RESENAS -->
            <h2>Reseñas registradas</h2>

            <div class="tablaContenedor">

                <asp:GridView
                    ID="gvResenas"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="tabla"
                    GridLines="None"
                    OnRowCommand="gvResenas_RowCommand"
                    EmptyDataText="No hay reseñas registradas.">

                    <Columns>

                        <asp:BoundField
                            DataField="ResenaId"
                            HeaderText="ID" />

                        <asp:BoundField
                            DataField="NombreActividad"
                            HeaderText="Actividad académica" />

                        <asp:BoundField
                            DataField="NombreEstudiante"
                            HeaderText="Estudiante" />

                        <asp:BoundField
                            DataField="Calificacion"
                            HeaderText="Calificación" />

                        <asp:BoundField
                            DataField="Comentario"
                            HeaderText="Comentario" />

                        <asp:BoundField
                            DataField="FechaCreacion"
                            HeaderText="Fecha"
                            DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                        <asp:TemplateField HeaderText="Acciones">

                            <ItemTemplate>

                                <div class="acciones">

                                    <asp:LinkButton
                                        ID="btnEditarFila"
                                        runat="server"
                                        Text="Modificar"
                                        CssClass="botonTabla botonEditarTabla"
                                        CommandName="SeleccionarResena"
                                        CommandArgument='<%# Eval("ResenaId") %>'
                                        CausesValidation="false" />

                                    <asp:LinkButton
                                        ID="btnEliminarFila"
                                        runat="server"
                                        Text="Eliminar"
                                        CssClass="botonTabla botonEliminarTabla"
                                        CommandName="EliminarResena"
                                        CommandArgument='<%# Eval("ResenaId") %>'
                                        CausesValidation="false"
                                        OnClientClick="return confirm('¿Está seguro de que desea eliminar esta reseña? Esta acción no se puede deshacer.');" />

                                </div>

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </form>

</body>
</html>
