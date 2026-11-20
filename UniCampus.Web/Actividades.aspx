
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Actividades.aspx.cs"
    Inherits="UniCampus.Web.Actividades" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>UniCampus - Actividades Academicas</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f6f9;
            margin: 30px;
        }

        h1, h2 {
            color: #203864;
        }

        .contenedor {
            background: white;
            padding: 25px;
            border-radius: 10px;
            max-width: 1100px;
            margin: auto;
        }

        .campo {
            margin-bottom: 15px;
        }

        .campo label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }

        .entrada {
            padding: 9px;
            width: 95%;
            max-width: 450px;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-sizing: border-box;
        }

        .boton {
            background-color: #203864;
            color: white;
            border: none;
            padding: 12px 25px;
            border-radius: 5px;
            cursor: pointer;
        }

        .botonCancelar {
            background-color: #777;
            color: white;
            border: none;
            padding: 12px 25px;
            border-radius: 5px;
            cursor: pointer;
            margin-left: 10px;
        }

        .botonEditar {
            background-color: #2874a6;
            color: white;
            border: none;
            padding: 7px 14px;
            border-radius: 5px;
            cursor: pointer;
            margin: 3px;
        }

        .botonEliminar {
            background-color: #c0392b;
            color: white;
            border: none;
            padding: 7px 14px;
            border-radius: 5px;
            cursor: pointer;
            margin: 3px;
        }

        .botonDetalles {
            background-color: #218c74;
            color: white;
            border: none;
            padding: 7px 14px;
            border-radius: 5px;
            cursor: pointer;
            margin: 3px;
        }

        .buscador {
            background-color: #f4f6f9;
            padding: 18px;
            border-radius: 8px;
            margin-bottom: 20px;
        }

        .buscadorControles {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 15px;
            margin-bottom: 15px;
        }

        .campoFiltro {
            display: flex;
            flex-direction: column;
            gap: 6px;
        }

        .campoFiltro label {
            font-weight: bold;
            color: #203864;
        }

        .entradaBuscar {
            padding: 11px;
            width: 100%;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-sizing: border-box;
        }

        .botonesFiltro {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
        }

        .botonBuscar {
            background-color: #2874a6;
            color: white;
            border: none;
            padding: 12px 22px;
            border-radius: 5px;
            cursor: pointer;
        }

        .botonMostrar {
            background-color: #777;
            color: white;
            border: none;
            padding: 12px 22px;
            border-radius: 5px;
            cursor: pointer;
        }

        .tablaContenedor {
            width: 100%;
            overflow-x: auto;
        }

        .tabla {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
        }

        .tabla th, .tabla td {
            border: 1px solid #ddd;
            padding: 10px;
            text-align: left;
        }

        .tabla th {
            background-color: #203864;
            color: white;
        }

        /* PANEL DE DETALLES */
        .panelDetalles {
            background-color: #f8fafc;
            border: 1px solid #d7e1ec;
            border-left: 5px solid #218c74;
            border-radius: 8px;
            padding: 22px;
            margin-top: 25px;
        }

        .panelDetalles h2 {
            margin-top: 0;
        }

        .detalleFila {
            padding: 10px 0;
            border-bottom: 1px solid #e1e5eb;
            overflow-wrap: anywhere;
        }

        .detalleFila strong {
            color: #203864;
        }

        .detalleDescripcion {
            white-space: pre-wrap;
        }

        .botonCerrar {
            background-color: #777;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
            margin-top: 18px;
        }

        /* MENU DE NAVEGACION */
        .menuNavegacion {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            background-color: #203864;
            padding: 12px;
            border-radius: 8px;
            margin-bottom: 25px;
        }

        .menuNavegacion a {
            color: white;
            text-decoration: none;
            padding: 11px 16px;
            border-radius: 5px;
            font-weight: bold;
        }

        .menuNavegacion a:hover {
            background-color: #35568d;
        }

        .menuNavegacion a.activo {
            background-color: #2874a6;
            color: white;
        }

        @media (max-width: 650px) {
            .buscadorControles {
                grid-template-columns: 1fr;
            }

            body {
                margin: 10px;
            }

            .contenedor {
                padding: 15px;
            }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="contenedor">

            <!-- MENU PRINCIPAL -->
            <nav class="menuNavegacion"
                 aria-label="Navegacion principal">

                <a href="Default.aspx">
                    Inicio
                </a>

                <a href="Actividades.aspx"
                   class="activo"
                   aria-current="page">
                    Actividades
                </a>

                <a href="Inscripciones.aspx">
                    Inscripciones
                </a>

                <a href="Resenas.aspx">
                    Reseñas
                </a>

                <a href="Estadisticas.aspx">
                    Estadísticas
                </a>

                <!-- SOLO DOCENTES -->
                <% if (Convert.ToString(Session["RolUsuario"]) == "Organizador") { %>

                    <a href="Estudiantes.aspx">
                        Estudiantes
                    </a>

                <% } %>

            </nav>

            <h1>UniCampus - Actividades Academicas</h1>

            <asp:Panel ID="pnlFormularioActividad"
                runat="server">

                <asp:HiddenField ID="hfActividadId"
                    runat="server" />

                <h2>
                    <asp:Label ID="lblTituloFormulario"
                        runat="server"
                        Text="Registrar nueva actividad" />
                </h2>

                <!-- FORMULARIO DE ACTIVIDADES -->

                <div class="campo">
                    <label>Titulo</label>

                    <asp:TextBox ID="txtTitulo"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="120" />
                </div>

                <div class="campo">
                    <label>Descripcion</label>

                    <asp:TextBox ID="txtDescripcion"
                        runat="server"
                        CssClass="entrada"
                        TextMode="MultiLine"
                        Rows="3" />
                </div>

                <div class="campo">
                    <label>Tipo de actividad</label>

                    <asp:DropDownList ID="ddlTipo"
                        runat="server"
                        CssClass="entrada">
                    </asp:DropDownList>
                </div>

                <div class="campo">
                    <label>Fecha de inicio</label>

                    <asp:TextBox ID="txtFechaInicio"
                        runat="server"
                        CssClass="entrada"
                        TextMode="Date" />
                </div>

                <div class="campo">
                    <label>Fecha de finalizacion</label>

                    <asp:TextBox ID="txtFechaFin"
                        runat="server"
                        CssClass="entrada"
                        TextMode="Date" />
                </div>

                <div class="campo">
                    <label>Hora de inicio</label>

                    <asp:TextBox ID="txtHoraInicio"
                        runat="server"
                        CssClass="entrada"
                        TextMode="Time" />
                </div>

                <div class="campo">
                    <label>Hora de finalizacion</label>

                    <asp:TextBox ID="txtHoraFin"
                        runat="server"
                        CssClass="entrada"
                        TextMode="Time" />
                </div>

                <div class="campo">
                    <label>Modalidad</label>

                    <asp:DropDownList ID="ddlModalidad"
                        runat="server"
                        CssClass="entrada">

                        <asp:ListItem
                            Text="Seleccione"
                            Value="" />

                        <asp:ListItem
                            Text="Presencial"
                            Value="Presencial" />

                        <asp:ListItem
                            Text="Virtual"
                            Value="Virtual" />

                        <asp:ListItem
                            Text="Hibrida"
                            Value="Hibrida" />

                    </asp:DropDownList>
                </div>

                <div class="campo">
                    <label>Lugar</label>

                    <asp:TextBox ID="txtLugar"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="150" />
                </div>

                <div class="campo">
                    <label>Cupos</label>

                    <asp:TextBox ID="txtCupos"
                        runat="server"
                        CssClass="entrada"
                        TextMode="Number" />
                </div>

                <div class="campo">
                    <label>Estado</label>

                    <asp:DropDownList ID="ddlEstado"
                        runat="server"
                        CssClass="entrada">

                        <asp:ListItem
                            Text="Borrador"
                            Value="Borrador" />

                        <asp:ListItem
                            Text="Publicada"
                            Value="Publicada" />

                        <asp:ListItem
                            Text="Cancelada"
                            Value="Cancelada" />

                        <asp:ListItem
                            Text="Finalizada"
                            Value="Finalizada" />

                    </asp:DropDownList>
                </div>

                <asp:Button ID="btnGuardar"
                    runat="server"
                    Text="Guardar actividad"
                    CssClass="boton"
                    OnClick="btnGuardar_Click" />

                <asp:Button ID="btnCancelar"
                    runat="server"
                    Text="Cancelar edicion"
                    CssClass="botonCancelar"
                    Visible="false"
                    CausesValidation="false"
                    OnClick="btnCancelar_Click" />

            </asp:Panel>

            <p>
                <asp:Label ID="lblMensaje"
                    runat="server"
                    ForeColor="Red" />
            </p>

            <hr />

            <h2>Listado de actividades</h2>

            <!-- BUSCADOR Y FILTROS -->
            <div class="buscador">

                <h3>Buscar y filtrar actividades</h3>

                <div class="buscadorControles">

                    <div class="campoFiltro">
                        <label>Nombre de actividad</label>

                        <asp:TextBox ID="txtBuscarActividad"
                            runat="server"
                            CssClass="entradaBuscar"
                            MaxLength="120"
                            placeholder="Ejemplo: Programacion" />
                    </div>

                    <div class="campoFiltro">
                        <label>Fecha de inicio</label>

                        <asp:TextBox ID="txtFiltroFecha"
                            runat="server"
                            CssClass="entradaBuscar"
                            TextMode="Date" />
                    </div>

                    <div class="campoFiltro">
                        <label>Tipo de actividad</label>

                        <asp:DropDownList ID="ddlFiltroTipo"
                            runat="server"
                            CssClass="entradaBuscar">
                        </asp:DropDownList>
                    </div>

                    <div class="campoFiltro">
                        <label>Estado</label>

                        <asp:DropDownList ID="ddlFiltroEstado"
                            runat="server"
                            CssClass="entradaBuscar">

                            <asp:ListItem
                                Text="Todos los estados"
                                Value="" />

                            <asp:ListItem
                                Text="Borrador"
                                Value="Borrador" />

                            <asp:ListItem
                                Text="Publicada"
                                Value="Publicada" />

                            <asp:ListItem
                                Text="Cancelada"
                                Value="Cancelada" />

                            <asp:ListItem
                                Text="Finalizada"
                                Value="Finalizada" />

                        </asp:DropDownList>
                    </div>

                </div>

                <div class="botonesFiltro">

                    <asp:Button ID="btnBuscar"
                        runat="server"
                        Text="Buscar / Filtrar"
                        CssClass="botonBuscar"
                        CausesValidation="false"
                        OnClick="btnBuscar_Click" />

                    <asp:Button ID="btnMostrarTodas"
                        runat="server"
                        Text="Mostrar todas"
                        CssClass="botonMostrar"
                        CausesValidation="false"
                        OnClick="btnMostrarTodas_Click" />

                </div>

            </div>

            <!-- TABLA DE ACTIVIDADES -->
            <div class="tablaContenedor">

                <asp:GridView ID="gvActividades"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataKeyNames="ActividadId"
                    CssClass="tabla"
                    OnRowCommand="gvActividades_RowCommand"
                    EmptyDataText="No se encontraron actividades.">

                    <Columns>

                        <asp:BoundField
                            DataField="ActividadId"
                            HeaderText="ID" />

                        <asp:BoundField
                            DataField="Titulo"
                            HeaderText="Actividad" />

                        <asp:BoundField
                            DataField="FechaInicio"
                            HeaderText="Fecha inicio"
                            DataFormatString="{0:dd/MM/yyyy}" />

                        <asp:BoundField
                            DataField="FechaFin"
                            HeaderText="Fecha fin"
                            DataFormatString="{0:dd/MM/yyyy}" />

                        <asp:BoundField
                            DataField="Modalidad"
                            HeaderText="Modalidad" />

                        <asp:BoundField
                            DataField="Cupos"
                            HeaderText="Cupos" />

                        <asp:BoundField
                            DataField="Estado"
                            HeaderText="Estado" />

                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>

                                <!-- VER DETALLES -->
                                <asp:Button ID="btnDetalles"
                                    runat="server"
                                    Text="Ver detalles"
                                    CssClass="botonDetalles"
                                    CommandName="VerDetalles"
                                    CommandArgument='<%# Eval("ActividadId") %>'
                                    CausesValidation="false" />

                                <!-- EDITAR -->
                                <asp:Button ID="btnEditar"
                                    runat="server"
                                    Text="Editar"
                                    CssClass="botonEditar"
                                    CommandName="EditarActividad"
                                    CommandArgument='<%# Eval("ActividadId") %>'
                                    CausesValidation="false" />

                                <!-- ELIMINAR -->
                                <asp:Button ID="btnEliminar"
                                    runat="server"
                                    Text="Eliminar"
                                    CssClass="botonEliminar"
                                    CommandName="EliminarActividad"
                                    CommandArgument='<%# Eval("ActividadId") %>'
                                    CausesValidation="false"
                                    OnClientClick="return confirm('¿Esta seguro de que desea eliminar esta actividad?');" />

                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

            <!-- PANEL PARA MOSTRAR DETALLES -->
            <asp:Panel ID="pnlDetalles"
                runat="server"
                CssClass="panelDetalles"
                Visible="false">

                <h2>Detalles de la actividad</h2>

                <div class="detalleFila">
                    <strong>ID:</strong>
                    <asp:Label ID="lblDetalleId" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Titulo:</strong>
                    <asp:Label ID="lblDetalleTitulo" runat="server" />
                </div>

                <div class="detalleFila detalleDescripcion">
                    <strong>Descripcion:</strong>
                    <asp:Label ID="lblDetalleDescripcion" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Tipo de actividad:</strong>
                    <asp:Label ID="lblDetalleTipo" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Fecha de inicio:</strong>
                    <asp:Label ID="lblDetalleFechaInicio" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Fecha de finalizacion:</strong>
                    <asp:Label ID="lblDetalleFechaFin" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Hora de inicio:</strong>
                    <asp:Label ID="lblDetalleHoraInicio" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Hora de finalizacion:</strong>
                    <asp:Label ID="lblDetalleHoraFin" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Modalidad:</strong>
                    <asp:Label ID="lblDetalleModalidad" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Lugar:</strong>
                    <asp:Label ID="lblDetalleLugar" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Cupos:</strong>
                    <asp:Label ID="lblDetalleCupos" runat="server" />
                </div>

                <div class="detalleFila">
                    <strong>Estado:</strong>
                    <asp:Label ID="lblDetalleEstado" runat="server" />
                </div>

                <asp:Button ID="btnCerrarDetalles"
                    runat="server"
                    Text="Cerrar detalles"
                    CssClass="botonCerrar"
                    CausesValidation="false"
                    OnClick="btnCerrarDetalles_Click" />

            </asp:Panel>

        </div>
    </form>
</body>
</html>
