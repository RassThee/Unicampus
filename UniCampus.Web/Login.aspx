
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="UniCampus.Web.Login" %>

<!DOCTYPE html>
<html lang="es">

<head runat="server">

    <meta charset="utf-8" />
    <meta name="viewport"
          content="width=device-width, initial-scale=1.0" />

    <title>UniCampus - Iniciar sesión</title>

    <style>

        body {
            font-family: Arial, sans-serif;
            background-color: #f4f6f9;
            margin: 0;
            padding: 20px;
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            box-sizing: border-box;
        }

        .login-contenedor {
            width: 100%;
            max-width: 420px;
            background-color: white;
            padding: 35px;
            border-radius: 12px;
            box-shadow: 0 4px 18px #cccccc;
            box-sizing: border-box;
        }

        .encabezado {
            text-align: center;
            margin-bottom: 30px;
        }

        .encabezado h1 {
            color: #203864;
            margin-bottom: 10px;
        }

        .encabezado p {
            color: #666666;
            font-size: 14px;
        }

        .campo {
            margin-bottom: 20px;
        }

        .campo label {
            display: block;
            font-weight: bold;
            margin-bottom: 8px;
            color: #333333;
        }

        .entrada {
            width: 100%;
            padding: 12px;
            border: 1px solid #cccccc;
            border-radius: 6px;
            box-sizing: border-box;
            font-size: 14px;
        }

        .entrada:focus {
            outline: none;
            border-color: #2874a6;
        }

        .boton {
            width: 100%;
            background-color: #203864;
            color: white;
            padding: 13px;
            border: none;
            border-radius: 6px;
            font-size: 15px;
            font-weight: bold;
            cursor: pointer;
        }

        .boton:hover {
            background-color: #365b94;
        }

        .mensaje {
            display: block;
            margin-top: 18px;
            text-align: center;
            color: #c0392b;
            font-size: 14px;
        }

        .pie {
            text-align: center;
            margin-top: 25px;
            color: #888888;
            font-size: 12px;
        }

    </style>

</head>

<body>

    <form id="form1" runat="server">

        <div class="login-contenedor">

            <div class="encabezado">

                <h1>UniCampus</h1>

                <p>
                    Sistema de Gestión de Actividades Académicas
                </p>

                <h2>Iniciar sesión</h2>

            </div>

            <!-- CORREO ELECTRONICO -->
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

            <!-- CONTRASEÑA -->
            <div class="campo">

                <label for="txtPassword">
                    Contraseña
                </label>

                <asp:TextBox
                    ID="txtPassword"
                    runat="server"
                    CssClass="entrada"
                    TextMode="Password">
                </asp:TextBox>

            </div>

            <!-- BOTON INICIAR SESION -->
            <asp:Button
                ID="btnIngresar"
                runat="server"
                Text="Iniciar sesión"
                CssClass="boton"
                OnClick="btnIngresar_Click" />

            <!-- MENSAJE DE ERROR -->
            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="mensaje">
            </asp:Label>

            <div class="pie">
                UniCampus - Sistema Académico
            </div>

        </div>

    </form>

</body>
</html>
