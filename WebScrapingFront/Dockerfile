# Utilizar una imagen de Node.js como base
FROM node:latest AS builder

# Establecer el directorio de trabajo en /app
WORKDIR /app

# Copiar los archivos de configuración
COPY . .

# Instalar las dependencias
RUN npm install

# Compilar la aplicación
RUN npm run build

# Etapa de producción
FROM nginx:latest

# Copiar los archivos estáticos de compilación de la etapa de compilación a la ubicación de nginx
COPY --from=builder /app/dist/* /usr/share/nginx/html/

# Exponer el puerto 80 para el servidor nginx
EXPOSE 80

# Comando para ejecutar nginx en primer plano
CMD ["nginx", "-g", "daemon off;"]