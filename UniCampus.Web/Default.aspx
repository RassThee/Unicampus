
<%@ Page Title="Inicio - UniCampus"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="UniCampus.Web._Default" %>

<asp:Content
    ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <style>
        .inicio-unicampus {
            font-family: Arial, sans-serif;
            color: #253047;
            padding: 25px 0;
        }

        /* USUARIO CONECTADO */
        .barra-usuario {
            background: #ffffff;
            border-radius: 12px;
            padding: 18px 25px;
            margin-bottom: 25px;
            box-shadow: 0 3px 12px rgba(0,0,0,0.08);
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 15px;
            flex-wrap: wrap;
        }

        .datos-usuario {
            display: flex;
            flex-direction: column;
            gap: 6px;
        }

        .saludo-usuario {
            color: #183b70;
            font-size: 18px;
            font-weight: bold;
        }

        .rol-usuario {
            color: #64748b;
            font-size: 14px;
        }

        .boton-salir {
            display: inline-block;
            background: #dc2626;
            color: white;
            padding: 11px 20px;
            border-radius: 7px;
            text-decoration: none;
            font-size: 14px;
            font-weight: bold;
        }

        .boton-salir:hover {
            background: #b91c1c;
            color: white;
            text-decoration: none;
        }

        /* BIENVENIDA */
        .bienvenida {
            background: linear-gradient(
                135deg,
                #183b70,
                #2874a6
            );
            color: white;
            padding: 55px 35px;
            border-radius: 14px;
            text-align: center;
            margin-bottom: 35px;
        }

        .bienvenida h1 {
            font-size: 42px;
            font-weight: bold;
            margin: 0 0 15px;
            color: white;
        }

        .bienvenida p {
            font-size: 18px;
            line-height: 1.7;
            max-width: 750px;
            margin: 0 auto;
            color: #eaf2ff;
        }

        .titulo-seccion {
            color: #183b70;
            font-size: 26px;
            font-weight: bold;
            margin-bottom: 12px;
            text-align: center;
        }

        .descripcion-seccion {
            text-align: center;
            color: #64748b;
            margin-bottom: 30px;
        }

        /* MODULOS */
        .modulos {
            display: grid;
            grid-template-columns:
                repeat(auto-fit, minmax(230px, 1fr));
            gap: 22px;
            margin-bottom: 40px;
        }

        .modulo {
            background: #ffffff;
            border-radius: 12px;
            padding: 30px 24px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.08);
            border-top: 5px solid #2563eb;
            text-align: center;
            transition: transform 0.2s ease,
                        box-shadow 0.2s ease;
        }

        .modulo:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 22px rgba(0,0,0,0.12);
        }

        .modulo h3 {
            color: #183b70;
            font-size: 21px;
            font-weight: bold;
            margin-bottom: 15px;
        }

        .modulo p {
            color: #64748b;
            font-size: 14px;
            line-height: 1.7;
            min-height: 75px;
        }

        .boton-modulo {
            display: inline-block;
            background-color: #2563eb;
            color: white;
            padding: 11px 22px;
            border-radius: 7px;
            text-decoration: none;
            font-weight: bold;
            margin-top: 15px;
        }

        .boton-modulo:hover {
            background-color: #1d4ed8;
            color: white;
            text-decoration: none;
        }

        .modulo-actividades {
            border-top-color: #2563eb;
        }

        .modulo-inscripciones {
            border-top-color: #16a34a;
        }

        .modulo-resenas {
            border-top-color: #ea580c;
        }

        .modulo-estadisticas {
            border-top-color: #9333ea;
        }

        /* INFORMACION */
        .informacion {
            background-color: #ffffff;
            border-radius: 12px;
            padding: 30px;
            box-shadow: 0 3px 12px rgba(0,0,0,0.06);
            margin-bottom: 25px;
        }

        .informacion h2 {
            color: #183b70;
            font-size: 23px;
            font-weight: bold;
            margin-top: 0;
        }

        .informacion p {
            color: #64748b;
            line-height: 1.8;
        }

        .pie-inicio {
            text-align: center;
            color: #64748b;
            padding: 20px;
            font-size: 14px;
        }

        @media (max-width: 650px) {
            .bienvenida {
                padding: 35px 20px;
            }

            .bienvenida h1 {
                font-size: 30px;
            }

            .bienvenida p {
                font-size: 16px;
            }

            .inicio-unicampus {
                padding: 15px 0;
            }

            .barra-usuario {
                flex-direction: column;
                align-items: stretch;
                text-align: center;
            }
        }
    </style>

    <main class="inicio-unicampus">

        <!-- USUARIO CONECTADO -->
        <section class="barra-usuario">

            <div class="datos-usuario">

                <asp:Label
                    ID="lblNombreUsuario"
                    runat="server"
                    CssClass="saludo-usuario"
                    Text="Bienvenido" />

                <asp:Label
                    ID="lblRolUsuario"
                    runat="server"
                    CssClass="rol-usuario"
                    Text="" />

            </div>

            <a href="CerrarSesion.aspx"
               class="boton-salir">
                Cerrar sesión
            </a>

        </section>

        <!-- BIENVENIDA -->
        <section class="bienvenida">

            <h1>Bienvenido a UniCampus</h1>

            <p>
                Sistema de gestión de actividades académicas
                diseñado para facilitar la administración
                de actividades, inscripciones, reseñas
                y estadísticas de la comunidad universitaria.
            </p>

        </section>

        <!-- MODULOS PRINCIPALES -->
        <section aria-labelledby="tituloModulos">

            <h2 id="tituloModulos" class="titulo-seccion">
                Módulos del sistema
            </h2>

            <p class="descripcion-seccion">
                Selecciona el módulo al que deseas ingresar.
            </p>

            <div class="modulos">

                <!-- ACTIVIDADES -->
                <article class="modulo modulo-actividades">

                    <h3>Actividades académicas</h3>

                    <p>
                        Consulta, registra, modifica y administra
                        las actividades académicas disponibles
                        en UniCampus.
                    </p>

                    <a href="Actividades.aspx"
                       class="boton-modulo">
                        Ir a Actividades
                    </a>

                </article>

                <!-- INSCRIPCIONES -->
                <article class="modulo modulo-inscripciones">

                    <h3>Inscripciones</h3>

                    <p>
                        Gestiona las inscripciones de estudiantes,
                        consulta los participantes y controla
                        los cupos de las actividades.
                    </p>

                    <a href="Inscripciones.aspx"
                       class="boton-modulo">
                        Ir a Inscripciones
                    </a>

                </article>

                <!-- RESENAS -->
                <article class="modulo modulo-resenas">

                    <h3>Reseñas y calificaciones</h3>

                    <p>
                        Registra y administra las opiniones
                        y calificaciones de los estudiantes
                        sobre las actividades académicas.
                    </p>

                    <a href="Resenas.aspx"
                       class="boton-modulo">
                        Ir a Reseñas
                    </a>

                </article>

                <!-- ESTADISTICAS -->
                <article class="modulo modulo-estadisticas">

                    <h3>Estadísticas</h3>

                    <p>
                        Consulta los indicadores generales,
                        las inscripciones, los cupos disponibles
                        y las calificaciones de las actividades.
                    </p>

                    <a href="Estadisticas.aspx"
                       class="boton-modulo">
                        Ir a Estadísticas
                    </a>

                </article>

            </div>

        </section>

        <!-- INFORMACION DEL SISTEMA -->
        <section class="informacion">

            <h2>Acerca de UniCampus</h2>

            <p>
                UniCampus es una aplicación web desarrollada
                con ASP.NET Web Forms, C# y SQL Server.
            </p>

            <p>
                Su objetivo es centralizar la gestión
                de actividades académicas, permitiendo
                administrar estudiantes, controlar
                inscripciones, registrar reseñas y consultar
                información estadística.
            </p>

            <p>
                El sistema utiliza una arquitectura
                organizada por capas para separar
                la presentación, la lógica de negocio
                y el acceso a los datos.
            </p>

        </section>

        <footer class="pie-inicio">
            UniCampus - Sistema de Gestión Académica
        </footer>

    </main>

</asp:Content>
