
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Inscripciones.aspx.cs"
    Inherits="UniCampus.Web.Inscripciones" %>

<!DOCTYPE html>
<html lang="es">

<head runat="server">

    <meta charset="utf-8" />

    <title>UniCampus - Inscripciones</title>

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

        .menu a.activo {
            background-color: #2874a6;
            color: white;
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
            max-width: 400px;
            padding: 10px;
            border: 1px solid #cccccc;
            border-radius: 5px;
            box-sizing: border-box;
        }

        .boton {
            background-color: #234a80;
            color: white;
            padding: 12px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .boton:hover {
            background-color: #16345d;
        }

        .botonCancelar {
            background-color: #c0392b;
            color: white;
            padding: 8px 14px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .botonCancelar:hover {
            background-color: #922b21;
        }

        .botonCancelar:disabled {
            background-color: #cccccc;
            cursor: not-allowed;
        }

        .mensaje {
            display: block;
            margin-top: 15px;
            font-weight: bold;
            color: #234a80;
        }

        /* PANEL DE CUPOS */
        .panel-cupos {
            background-color: #f8faff;
            border: 1px solid #dce5f2;
            border-radius: 8px;
            padding: 20px;
            margin: 20px 0;
        }

        .panel-cupos h3 {
            color: #234a80;
            margin-top: 0;
            margin-bottom: 15px;
        }

        .contenedor-cupos {
            display: flex;
            flex-wrap: wrap;
            gap: 15px;
        }

        .tarjeta-cupo {
            flex: 1;
            min-width: 150px;
            background-color: white;
            border: 1px solid #dddddd;
            border-radius: 8px;
            padding: 18px;
            text-align: center;
            box-sizing: border-box;
        }

        .numero-cupo {
            display: block;
            font-size: 30px;
            font-weight: bold;
            color: #234a80;
            margin-bottom: 8px;
        }

        .texto-cupo {
            display: block;
            font-size: 14px;
            color: #555555;
            font-weight: bold;
        }

        /* TABLA DE INSCRIPCIONES */
        .tabla {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }

        .tabla th {
            background-color: #234a80;
            color: white;
            padding: 12px;
            text-align: left;
        }

        .tabla td {
            padding: 12px;
            border-bottom: 1px solid #dddddd;
            text-align: left;
        }

        .tabla tr:hover td {
            background-color: #f4f6f9;
        }

        .tabla-contenedor {
            overflow-x: auto;
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

            .tarjeta-cupo {
                min-width: 100%;
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

                <a href="Inscripciones.aspx"
                   class="activo"
                   aria-current="page">
                    Inscripciones
                </a>

                <a href="Resenas.aspx">
                    Reseñas
                </a>

                <!-- ESTADISTICAS PARA AMBOS ROLES -->
                <a id="lnkEstadisticas"
                   runat="server"
                   href="Estadisticas.aspx">
                    Estadísticas
                </a>

                <!-- ESTUDIANTES SOLO PARA DOCENTES -->
                <a id="lnkEstudiantes"
                   runat="server"
                   href="Estudiantes.aspx">
                    Estudiantes
                </a>

            </nav>

            <h1>UniCampus</h1>

            <h2>Registro de inscripciones</h2>

            <!-- SELECCIONAR ACTIVIDAD -->
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

            <!-- INFORMACION DE CUPOS -->
            <div class="panel-cupos">

                <h3>Disponibilidad de la actividad</h3>

                <div class="contenedor-cupos">

                    <!-- CUPOS TOTALES -->
                    <div class="tarjeta-cupo">

                        <asp:Label
                            ID="lblCuposTotales"
                            runat="server"
                            Text="0"
                            CssClass="numero-cupo">
                        </asp:Label>

                        <span class="texto-cupo">
                            Cupos totales
                        </span>

                    </div>

                    <!-- ESTUDIANTES INSCRITOS -->
                    <div class="tarjeta-cupo">

                        <asp:Label
                            ID="lblInscritos"
                            runat="server"
                            Text="0"
                            CssClass="numero-cupo">
                        </asp:Label>

                        <span class="texto-cupo">
                            Inscritos
                        </span>

                    </div>

                    <!-- CUPOS DISPONIBLES -->
                    <div class="tarjeta-cupo">

                        <asp:Label
                            ID="lblDisponibles"
                            runat="server"
                            Text="0"
                            CssClass="numero-cupo">
                        </asp:Label>

                        <span class="texto-cupo">
                            Cupos disponibles
                        </span>

                    </div>

                </div>

            </div>

            <!-- SELECCIONAR ESTUDIANTE -->
            <asp:Panel
                ID="pnlSeleccionEstudiante"
                runat="server">

                <div class="campo">

                    <label>Nombre del estudiante</label>

                    <asp:DropDownList
                        ID="ddlEstudiante"
                        runat="server"
                        CssClass="entrada">
                    </asp:DropDownList>

                </div>

            </asp:Panel>

            <!-- BOTON DE REGISTRO -->
            <asp:Button
                ID="btnInscribir"
                runat="server"
                Text="Registrar inscripción"
                CssClass="boton"
                OnClick="btnInscribir_Click" />

            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="mensaje">
            </asp:Label>

            <hr />

            <!-- LISTADO DE INSCRIPCIONES -->
            <h2>Estudiantes inscritos en actividades</h2>

            <div class="tabla-contenedor">

                <asp:GridView
                    ID="gvInscripciones"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="tabla"
                    GridLines="None"
                    OnRowCommand="gvInscripciones_RowCommand"
                    EmptyDataText="No hay inscripciones registradas.">

                    <Columns>

                        <asp:BoundField
                            DataField="InscripcionId"
                            HeaderText="ID" />

                        <asp:BoundField
                            DataField="NombreActividad"
                            HeaderText="Actividad académica" />

                        <asp:BoundField
                            DataField="NombreEstudiante"
                            HeaderText="Estudiante" />

                        <asp:BoundField
                            DataField="Programa"
                            HeaderText="Programa académico"
                            NullDisplayText="Sin programa" />

                        <asp:BoundField
                            DataField="FechaInscripcion"
                            HeaderText="Fecha de inscripción"
                            DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                        <asp:BoundField
                            DataField="Estado"
                            HeaderText="Estado" />

                        <asp:TemplateField HeaderText="Acción">

                            <ItemTemplate>

                                <asp:Button
                                    ID="btnCancelarInscripcion"
                                    runat="server"
                                    Text="Cancelar"
                                    CssClass="botonCancelar"
                                    CommandName="CancelarInscripcion"
                                    CommandArgument='<%# Eval("InscripcionId") %>'
                                    Enabled='<%# Eval("Estado").ToString() == "Activa" %>'
                                    OnClientClick="return confirm('¿Está seguro de cancelar esta inscripción?');" />

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </form>

</body>
</html>
