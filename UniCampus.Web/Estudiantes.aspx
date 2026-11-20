
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Estudiantes.aspx.cs"
    Inherits="UniCampus.Web.Estudiantes" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">

    <meta charset="utf-8" />
    <meta name="viewport"
          content="width=device-width, initial-scale=1.0" />

    <title>UniCampus - Estudiantes</title>

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
            padding: 10px 15px;
            border-radius: 5px;
        }

        .menu a:hover:not(.activo) {
            background-color: #365b94;
        }

        .menu a.activo {
            background-color: #2874a6;
            color: white;
            font-weight: bold;
        }

        /* TITULOS */
        h1 {
            color: #203864;
            margin-bottom: 10px;
        }

        h2 {
            color: #203864;
            margin-top: 25px;
        }

        /* FORMULARIO */
        .formulario {
            background-color: #f8f9fc;
            border: 1px solid #dddddd;
            padding: 20px;
            border-radius: 8px;
            margin-top: 20px;
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
            max-width: 500px;
            padding: 10px;
            border: 1px solid #cccccc;
            border-radius: 5px;
            box-sizing: border-box;
        }

        .entrada:focus {
            outline: none;
            border-color: #2874a6;
        }

        /* CONTRASEÑAS */
        .password-contenedor {
            display: flex;
            align-items: center;
            gap: 8px;
            max-width: 500px;
        }

        .password-contenedor .entrada {
            flex: 1;
            min-width: 0;
        }

        .boton-ojo {
            padding: 9px 13px;
            background-color: #e5ebf4;
            color: #203864;
            border: 1px solid #cccccc;
            border-radius: 5px;
            cursor: pointer;
            font-size: 17px;
        }

        .boton-ojo:hover {
            background-color: #d1ddeb;
        }

        .ayuda {
            display: block;
            margin-top: 6px;
            color: #666666;
            font-size: 13px;
        }

        /* BOTONES */
        .boton {
            background-color: #203864;
            color: white;
            padding: 11px 18px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            margin-right: 8px;
            margin-bottom: 8px;
        }

        .boton:hover {
            background-color: #365b94;
        }

        .boton-cancelar {
            background-color: #777777;
        }

        .boton-cancelar:hover {
            background-color: #555555;
        }

        .boton-password {
            background-color: #a66a12;
        }

        .boton-password:hover {
            background-color: #84520d;
        }

        /* MENSAJES */
        .mensaje {
            display: block;
            margin-top: 15px;
            font-weight: bold;
        }

        /* PANEL RESTABLECER CONTRASEÑA */
        .panel-password {
            background-color: #fffaf0;
            border: 1px solid #e6d5a8;
            border-radius: 8px;
            padding: 20px;
            margin-top: 25px;
        }

        .panel-password h2 {
            margin-top: 0;
        }

        /* TABLA DE ESTUDIANTES */
        .tabla {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .tabla th {
            background-color: #203864;
            color: white;
            padding: 12px;
            text-align: left;
        }

        .tabla td {
            padding: 12px;
            border-bottom: 1px solid #dddddd;
        }

        .tabla tr:nth-child(even) {
            background-color: #f5f7fa;
        }

        /* ACCIONES DE LA TABLA */
        .accion {
            display: inline-block;
            padding: 7px 10px;
            margin: 3px;
            border-radius: 4px;
            color: white;
            text-decoration: none;
            font-size: 13px;
        }

        .accion:hover {
            color: white;
            opacity: 0.9;
        }

        .modificar {
            background-color: #2864a0;
        }

        .desactivar {
            background-color: #c0392b;
        }

        .reactivar {
            background-color: #218c52;
        }

        .restablecer {
            background-color: #a66a12;
        }

        .tabla-contenedor {
            overflow-x: auto;
        }

        /* DISEÑO PARA CELULARES */
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

            .entrada {
                max-width: 100%;
            }
        }
    </style>

    <script type="text/javascript">

        // MOSTRAR U OCULTAR CONTRASEÑA
        function mostrarPassword(idCampo, idBoton) {

            var campo = document.getElementById(idCampo);
            var boton = document.getElementById(idBoton);

            if (!campo || !boton) {
                return;
            }

            if (campo.type === "password") {
                campo.type = "text";
                boton.textContent = "🙈";
                boton.setAttribute(
                    "aria-label", "Ocultar contraseña");
            }
            else {
                campo.type = "password";
                boton.textContent = "👁";
                boton.setAttribute(
                    "aria-label", "Mostrar contraseña");
            }
        }

    </script>

</head>

<body>

    <form id="form1" runat="server">

        <div class="contenedor">

            <!-- MENU DE NAVEGACION -->
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

                <a href="Estadisticas.aspx">
                    Estadísticas
                </a>

                <a href="Estudiantes.aspx"
                   class="activo"
                   aria-current="page">
                    Estudiantes
                </a>

            </nav>

            <!-- TITULO PRINCIPAL -->
            <h1>Administración de estudiantes</h1>

            <p>
                Registre, consulte y administre los estudiantes
                de UniCampus.
            </p>

            <!-- FORMULARIO DE ESTUDIANTES -->
            <div class="formulario">

                <h2>Datos del estudiante</h2>

                <!-- NOMBRE -->
                <div class="campo">

                    <label for="txtNombre">
                        Nombre completo
                    </label>

                    <asp:TextBox
                        ID="txtNombre"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="100">
                    </asp:TextBox>

                </div>

                <!-- CORREO -->
                <div class="campo">

                    <label for="txtCorreo">
                        Correo electrónico
                    </label>

                    <asp:TextBox
                        ID="txtCorreo"
                        runat="server"
                        CssClass="entrada"
                        TextMode="Email"
                        MaxLength="200">
                    </asp:TextBox>

                </div>

                <!-- PROGRAMA -->
                <div class="campo">

                    <label for="txtPrograma">
                        Programa académico
                    </label>

                    <asp:TextBox
                        ID="txtPrograma"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="150">
                    </asp:TextBox>

                </div>

                <!-- CONTRASEÑA INICIAL -->
                <asp:Panel
                    ID="pnlPasswordInicial"
                    runat="server">

                    <div class="campo">

                        <label for="txtPassword">
                            Contraseña inicial
                        </label>

                        <div class="password-contenedor">

                            <asp:TextBox
                                ID="txtPassword"
                                runat="server"
                                CssClass="entrada"
                                TextMode="Password"
                                MaxLength="128">
                            </asp:TextBox>

                            <button
                                type="button"
                                id="btnOjoPassword"
                                class="boton-ojo"
                                aria-label="Mostrar contraseña"
                                onclick="mostrarPassword('txtPassword', 'btnOjoPassword')">
                                👁
                            </button>

                        </div>

                        <span class="ayuda">
                            La contraseña debe tener entre
                            8 y 128 caracteres.
                        </span>

                    </div>

                    <!-- CONFIRMAR CONTRASEÑA -->
                    <div class="campo">

                        <label for="txtConfirmarPassword">
                            Confirmar contraseña
                        </label>

                        <div class="password-contenedor">

                            <asp:TextBox
                                ID="txtConfirmarPassword"
                                runat="server"
                                CssClass="entrada"
                                TextMode="Password"
                                MaxLength="128">
                            </asp:TextBox>

                            <button
                                type="button"
                                id="btnOjoConfirmar"
                                class="boton-ojo"
                                aria-label="Mostrar contraseña"
                                onclick="mostrarPassword('txtConfirmarPassword', 'btnOjoConfirmar')">
                                👁
                            </button>

                        </div>

                    </div>

                </asp:Panel>

                <!-- BOTON REGISTRAR O GUARDAR CAMBIOS -->
                <asp:Button
                    ID="btnRegistrar"
                    runat="server"
                    Text="Registrar estudiante"
                    CssClass="boton"
                    OnClick="btnRegistrar_Click" />

                <!-- BOTON CANCELAR MODIFICACION -->
                <asp:Button
                    ID="btnCancelar"
                    runat="server"
                    Text="Cancelar modificación"
                    CssClass="boton boton-cancelar"
                    CausesValidation="false"
                    OnClick="btnCancelar_Click" />

                <!-- MENSAJES -->
                <asp:Label
                    ID="lblMensaje"
                    runat="server"
                    CssClass="mensaje">
                </asp:Label>

            </div>

            <!-- PANEL PARA RESTABLECER CONTRASEÑA -->
            <asp:Panel
                ID="pnlRestablecerPassword"
                runat="server"
                CssClass="panel-password"
                Visible="false">

                <h2>Restablecer contraseña</h2>

                <p>
                    Estudiante seleccionado:
                    <strong>
                        <asp:Label
                            ID="lblEstudiantePassword"
                            runat="server">
                        </asp:Label>
                    </strong>
                </p>

                <p>
                    Escriba una contraseña nueva.
                    La contraseña anterior dejará de funcionar.
                </p>

                <!-- NUEVA CONTRASEÑA -->
                <div class="campo">

                    <label for="txtNuevaPassword">
                        Nueva contraseña
                    </label>

                    <div class="password-contenedor">

                        <asp:TextBox
                            ID="txtNuevaPassword"
                            runat="server"
                            CssClass="entrada"
                            TextMode="Password"
                            MaxLength="128">
                        </asp:TextBox>

                        <button
                            type="button"
                            id="btnOjoNueva"
                            class="boton-ojo"
                            aria-label="Mostrar contraseña"
                            onclick="mostrarPassword('txtNuevaPassword', 'btnOjoNueva')">
                            👁
                        </button>

                    </div>

                </div>

                <!-- CONFIRMAR NUEVA CONTRASEÑA -->
                <div class="campo">

                    <label for="txtConfirmarNuevaPassword">
                        Confirmar nueva contraseña
                    </label>

                    <div class="password-contenedor">

                        <asp:TextBox
                            ID="txtConfirmarNuevaPassword"
                            runat="server"
                            CssClass="entrada"
                            TextMode="Password"
                            MaxLength="128">
                        </asp:TextBox>

                        <button
                            type="button"
                            id="btnOjoNuevaConfirmar"
                            class="boton-ojo"
                            aria-label="Mostrar contraseña"
                            onclick="mostrarPassword('txtConfirmarNuevaPassword', 'btnOjoNuevaConfirmar')">
                            👁
                        </button>

                    </div>

                </div>

                <!-- GUARDAR NUEVA CONTRASEÑA -->
                <asp:Button
                    ID="btnGuardarPassword"
                    runat="server"
                    Text="Guardar nueva contraseña"
                    CssClass="boton boton-password"
                    OnClick="btnGuardarPassword_Click" />

                <!-- CANCELAR RESTABLECIMIENTO -->
                <asp:Button
                    ID="btnCancelarPassword"
                    runat="server"
                    Text="Cancelar"
                    CssClass="boton boton-cancelar"
                    CausesValidation="false"
                    OnClick="btnCancelarPassword_Click" />

                <asp:Label
                    ID="lblMensajePassword"
                    runat="server"
                    CssClass="mensaje">
                </asp:Label>

            </asp:Panel>

            <!-- LISTADO DE ESTUDIANTES -->
            <h2>Estudiantes registrados</h2>

            <div class="tabla-contenedor">

                <asp:GridView
                    ID="gvEstudiantes"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="tabla"
                    GridLines="None"
                    EmptyDataText="No hay estudiantes registrados."
                    OnRowCommand="gvEstudiantes_RowCommand">

                    <Columns>

                        <asp:BoundField
                            DataField="UsuarioId"
                            HeaderText="ID" />

                        <asp:BoundField
                            DataField="Nombre"
                            HeaderText="Nombre" />

                        <asp:BoundField
                            DataField="Correo"
                            HeaderText="Correo electrónico" />

                        <asp:BoundField
                            DataField="Programa"
                            HeaderText="Programa académico" />

                        <asp:BoundField
                            DataField="Estado"
                            HeaderText="Estado" />

                        <asp:TemplateField HeaderText="Modificar">

                            <ItemTemplate>

                                <asp:LinkButton
                                    ID="btnModificar"
                                    runat="server"
                                    Text="Modificar"
                                    CssClass="accion modificar"
                                    CommandName="ModificarEstudiante"
                                    CommandArgument='<%# Eval("UsuarioId") %>'
                                    CausesValidation="false" />

                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Estado">

                            <ItemTemplate>

                                <asp:LinkButton
                                    ID="btnDesactivar"
                                    runat="server"
                                    Text="Desactivar"
                                    CssClass="accion desactivar"
                                    CommandName="DesactivarEstudiante"
                                    CommandArgument='<%# Eval("UsuarioId") %>'
                                    CausesValidation="false"
                                    Visible='<%# Eval("Estado").ToString() == "Activo" %>'
                                    OnClientClick="return confirm('¿Desea desactivar este estudiante?');" />

                                <asp:LinkButton
                                    ID="btnReactivar"
                                    runat="server"
                                    Text="Reactivar"
                                    CssClass="accion reactivar"
                                    CommandName="ReactivarEstudiante"
                                    CommandArgument='<%# Eval("UsuarioId") %>'
                                    CausesValidation="false"
                                    Visible='<%# Eval("Estado").ToString() == "Inactivo" %>'
                                    OnClientClick="return confirm('¿Desea reactivar este estudiante?');" />

                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Contraseña">

                            <ItemTemplate>

                                <asp:LinkButton
                                    ID="btnRestablecerPassword"
                                    runat="server"
                                    Text="Restablecer contraseña"
                                    CssClass="accion restablecer"
                                    CommandName="RestablecerPassword"
                                    CommandArgument='<%# Eval("UsuarioId") %>'
                                    CausesValidation="false" />

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </form>

</body>
</html> 
