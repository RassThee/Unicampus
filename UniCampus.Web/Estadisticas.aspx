
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Estadisticas.aspx.cs"
    Inherits="UniCampus.Web.Estadisticas" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />

    <title>Estadísticas - UniCampus</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f3f5f9;
            margin: 0;
            padding: 25px;
            color: #253047;
        }

        .contenedor {
            max-width: 1150px;
            margin: auto;
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
            background-color: #35568d;
        }

        .menu a.activo {
            background-color: #2874a6;
            color: white;
        }

        h1 {
            color: #183b70;
            margin-bottom: 8px;
        }

        .subtitulo {
            color: #64748b;
            margin-bottom: 25px;
        }

        /* TARJETAS DE ESTADISTICAS */
        .tarjetas {
            display: grid;
            grid-template-columns:
                repeat(auto-fit, minmax(220px, 1fr));
            gap: 18px;
            margin-bottom: 30px;
        }

        .tarjeta {
            background: white;
            padding: 24px;
            border-radius: 12px;
            box-shadow: 0 3px 12px rgba(0,0,0,0.07);
            border-left: 5px solid #2563eb;
        }

        .tarjeta h3 {
            font-size: 15px;
            color: #64748b;
            margin: 0 0 12px;
        }

        .numero {
            display: block;
            font-size: 32px;
            font-weight: bold;
            color: #183b70;
        }

        /* COLORES PARA ESTUDIANTES */
        .tarjeta-activos {
            border-left-color: #16a34a;
        }

        .tarjeta-inactivos {
            border-left-color: #ea580c;
        }

        /* SECCIONES DE TABLAS */
        .seccion {
            background: white;
            padding: 22px;
            border-radius: 12px;
            margin-bottom: 25px;
            box-shadow: 0 3px 12px rgba(0,0,0,0.06);
            overflow-x: auto;
        }

        .seccion h2 {
            color: #183b70;
            font-size: 20px;
            margin-top: 0;
        }

        .tabla {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
        }

        .tabla th {
            background-color: #183b70;
            color: white;
            padding: 12px;
            text-align: left;
        }

        .tabla td {
            padding: 11px;
            border-bottom: 1px solid #e2e8f0;
        }

        .tabla tr:nth-child(even) {
            background-color: #f8fafc;
        }

        .mensaje {
            display: block;
            color: #b91c1c;
            margin-bottom: 15px;
            font-weight: bold;
        }

        .boton {
            background-color: #2563eb;
            color: white;
            border: none;
            padding: 11px 20px;
            border-radius: 7px;
            cursor: pointer;
            margin-bottom: 20px;
        }

        .boton:hover {
            background-color: #1d4ed8;
        }

        @media (max-width: 650px) {
            body {
                padding: 10px;
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

                <a href="Resenas.aspx">
                    Reseñas
                </a>

                <a href="Estadisticas.aspx"
                   class="activo"
                   aria-current="page">
                    Estadísticas
                </a>

                <!-- ESTUDIANTES SOLO PARA DOCENTES -->
                <% if (Convert.ToString(Session["RolUsuario"]) == "Organizador") { %>

                    <a href="Estudiantes.aspx">
                        Estudiantes
                    </a>

                <% } %>

            </nav>

            <!-- TITULO PERSONALIZADO -->
            <h1>
                <asp:Label
                    ID="lblTituloEstadisticas"
                    runat="server"
                    Text="Panel de estadísticas" />
            </h1>

            <!-- SUBTITULO PERSONALIZADO -->
            <p class="subtitulo">

                <asp:Label
                    ID="lblSubtituloEstadisticas"
                    runat="server"
                    Text="Resumen general de las actividades académicas de UniCampus." />

            </p>

            <!-- MENSAJES -->
            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="mensaje" />

            <!-- BOTON ACTUALIZAR -->
            <asp:Button
                ID="btnActualizar"
                runat="server"
                Text="Actualizar estadísticas"
                CssClass="boton"
                OnClick="btnActualizar_Click" />

            <!-- TARJETAS DE RESUMEN -->
            <div class="tarjetas">

                <!-- TOTAL DE ACTIVIDADES -->
                <div class="tarjeta">

                    <h3>Actividades disponibles</h3>

                    <asp:Label
                        ID="lblTotalActividades"
                        runat="server"
                        Text="0"
                        CssClass="numero" />

                </div>

                <!-- ACTIVIDADES PUBLICADAS -->
                <div class="tarjeta">

                    <h3>Actividades publicadas</h3>

                    <asp:Label
                        ID="lblActividadesPublicadas"
                        runat="server"
                        Text="0"
                        CssClass="numero" />

                </div>

                <!-- INSCRIPCIONES ACTIVAS -->
                <div class="tarjeta">

                    <h3>Inscripciones activas</h3>

                    <asp:Label
                        ID="lblInscripcionesActivas"
                        runat="server"
                        Text="0"
                        CssClass="numero" />

                </div>

                <!-- INSCRIPCIONES CANCELADAS -->
                <div class="tarjeta">

                    <h3>Inscripciones canceladas</h3>

                    <asp:Label
                        ID="lblInscripcionesCanceladas"
                        runat="server"
                        Text="0"
                        CssClass="numero" />

                </div>

                <!-- RESENAS -->
                <div class="tarjeta">

                    <h3>Reseñas registradas</h3>

                    <asp:Label
                        ID="lblTotalResenas"
                        runat="server"
                        Text="0"
                        CssClass="numero" />

                </div>

                <!-- PROMEDIO DE CALIFICACIONES -->
                <div class="tarjeta">

                    <h3>Promedio de calificaciones</h3>

                    <asp:Label
                        ID="lblPromedioCalificacion"
                        runat="server"
                        Text="0"
                        CssClass="numero" />

                </div>

                <!-- ESTUDIANTES ACTIVOS: SOLO DOCENTES -->
                <div
                    id="tarjetaEstudiantesActivos"
                    runat="server"
                    class="tarjeta tarjeta-activos">

                    <h3>Estudiantes activos</h3>

                    <asp:Label
                        ID="lblEstudiantesActivos"
                        runat="server"
                        Text="0"
                        CssClass="numero" />

                </div>

                <!-- ESTUDIANTES INACTIVOS: SOLO DOCENTES -->
                <div
                    id="tarjetaEstudiantesInactivos"
                    runat="server"
                    class="tarjeta tarjeta-inactivos">

                    <h3>Estudiantes inactivos</h3>

                    <asp:Label
                        ID="lblEstudiantesInactivos"
                        runat="server"
                        Text="0"
                        CssClass="numero" />

                </div>

            </div>

            <!-- INSCRIPCIONES POR ACTIVIDAD -->
            <div class="seccion">

                <h2>
                    <asp:Label
                        ID="lblTituloInscripciones"
                        runat="server"
                        Text="Inscripciones y cupos por actividad" />
                </h2>

                <asp:GridView
                    ID="gvInscripciones"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="tabla"
                    GridLines="None"
                    EmptyDataText="No hay actividades registradas.">

                    <Columns>

                        <asp:BoundField
                            DataField="Titulo"
                            HeaderText="Actividad" />

                        <asp:BoundField
                            DataField="Cupos"
                            HeaderText="Cupos totales" />

                        <asp:BoundField
                            DataField="InscripcionesActivas"
                            HeaderText="Inscritos" />

                        <asp:BoundField
                            DataField="CuposDisponibles"
                            HeaderText="Disponibles" />

                    </Columns>

                </asp:GridView>

            </div>

            <!-- ACTIVIDADES POR ESTADO -->
            <div class="seccion">

                <h2>
                    <asp:Label
                        ID="lblTituloEstados"
                        runat="server"
                        Text="Actividades por estado" />
                </h2>

                <asp:GridView
                    ID="gvEstados"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="tabla"
                    GridLines="None"
                    EmptyDataText="No hay datos disponibles.">

                    <Columns>

                        <asp:BoundField
                            DataField="Estado"
                            HeaderText="Estado" />

                        <asp:BoundField
                            DataField="Cantidad"
                            HeaderText="Cantidad" />

                    </Columns>

                </asp:GridView>

            </div>

            <!-- CALIFICACIONES POR ACTIVIDAD -->
            <div class="seccion">

                <h2>
                    <asp:Label
                        ID="lblTituloCalificaciones"
                        runat="server"
                        Text="Calificaciones por actividad" />
                </h2>

                <asp:GridView
                    ID="gvCalificaciones"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="tabla"
                    GridLines="None"
                    EmptyDataText="No hay actividades registradas.">

                    <Columns>

                        <asp:BoundField
                            DataField="Titulo"
                            HeaderText="Actividad" />

                        <asp:BoundField
                            DataField="TotalResenas"
                            HeaderText="Reseñas" />

                        <asp:BoundField
                            DataField="PromedioCalificacion"
                            HeaderText="Promedio"
                            DataFormatString="{0:N2}" />

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </form>

</body>
</html>
