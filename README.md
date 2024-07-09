# TFG-Aplicación web para gestión de indicadores del portal de investigación

## Development server
La plataforma gira en torno a la información referente a los investigadores de Universidad de Burgos. Se encuentran los departamentos, centros de investigación, áreas del conocimiento y programas de doctorado. Dentro de cada una de estas unidades, aparece una breve descripción y se encuentran los investigadores que pertenecen al grupo.

La producción científica de los investigadores de la UBU, en gran medida, es fruto de colaboraciones con otros investigadores. Pero si el investigador que ha colaborado, no está registrado dentro del portal de investigación de la UBU, no se existe una forma de obtener la información referente a este investigador.

Con la realización de este proyecto se pretende unificar la producción científica de los investigadores que no están registrados en el portal de investigación, con el fin de tener las publicaciones y sus indicadores disponibles para su visualización. También facilitar la elaboración de informes al coordinador de doctorado, gracias a esta agrupación de la información del portal.

# Instalación:

Como prerequisito hay que tener instalado Docker Desktop.
Con el directorio del proyecto descargado, abrimos un terminal y accedemos al directorio raíz del proyecto.
Escribimos el comando: 

docker-compose up --build

Esperamos a que se generen los contenedores en Docker y se ejecuten las migraciones a la base de datos. 

Al terminar la compilacion tendremos la aplicación disponible en http://localhost:4200/investigadores

# Demo de la aplicación web:

https://youtu.be/ZB_T0Q1Q6Uo
