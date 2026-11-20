
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ConfigurarAccesos.aspx.cs"
    Inherits="UniCampus.Web.ConfigurarAccesos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Configuración inicial - UniCampus</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background: #eef3f9;
            margin: 0;
            padding: 0;
        }

        .contenedor {
            width: 420px;
            max-width: 90%;
            margin: 90px auto;
            background: white;
            padding: 35px;
            border-radius: 12px;
            box-shadow: 0 4px 18px rgba(0,0,0,0.12);
        }

        h2 {
            color: #174a85;
            text-align: center;
            margin-bottom: 10px;
        }

        .descripcion {
            color: #666;
            text-align: center;
            font-size: 14px;
            margin-bottom: 25px;
        }

        .etiqueta {
            display: block;
            font-weight: bold;
            margin-top: 18px;
            margin-bottom: 8px;
        }

        .entrada {
            width: 100%;
            padding: 11px;
            border: 1px solid #ccc;
            border-radius: 6px;
            box-sizing: border-box;
            font-size: 14px;
        }

        .boton {
            width: 100%;
            margin-top: 25px;
            padding: 12px;
            background: #174a85;
            color: white;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 15px;
            font-weight: bold;
        }

        .boton:hover {
            background: #10365f;
        }

        .mensaje {
            display: block;
            margin-top: 18px;
            text-align: center;
            font-weight: bold;
            line-height: 1.5;
        }

        .nota {
            margin-top: 22px;
            font-size: 12px;
            color: #777;
            text-align: center;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <div class="contenedor">

            <h2>UniCampus</h2>

            <p class="descripcion">
                Configuración inicial de contraseñas
            </p>

            <asp:Label
                ID="lblUsuario"
                runat="server"
                Text="Seleccionar usuario:"
                CssClass="etiqueta" />

            <asp:DropDownList
                ID="ddlUsuarios"
                runat="server"
                CssClass="entrada">

                <asp:ListItem Text="-- Seleccione un usuario --"
                    Value="0" />

                <asp:ListItem Text="Carlos Perez - Participante"
                    Value="1" />

                <asp:ListItem Text="Maria Gomez - Participante"
                    Value="2" />

                <asp:ListItem Text="Ana Martínez - Organizador"
                    Value="3" />

                <asp:ListItem Text="Pedro Ramirez - Participante"
                    Value="4" />

                <asp:ListItem Text="Laura Rodriguez - Participante"
                    Value="5" />

            </asp:DropDownList>

            <asp:Label
                ID="lblPassword"
                runat="server"
                Text="Contraseña:"
                CssClass="etiqueta" />

            <asp:TextBox
                ID="txtPassword"
                runat="server"
                CssClass="entrada"
                TextMode="Password"
                MaxLength="100" />

            <asp:Label
                ID="lblConfirmar"
                runat="server"
                Text="Confirmar contraseña:"
                CssClass="etiqueta" />

            <asp:TextBox
                ID="txtConfirmarPassword"
                runat="server"
                CssClass="entrada"
                TextMode="Password"
                MaxLength="100" />

            <asp:Button
                ID="btnGuardar"
                runat="server"
                Text="Guardar contraseña"
                CssClass="boton"
                OnClick="btnGuardar_Click" />

            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="mensaje" />

            <p class="nota">
                Página temporal para configurar usuarios
                de prueba. Eliminar después de utilizarla.
            </p>

        </div>

    </form>
</body>
</html>
