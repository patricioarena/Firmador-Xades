# ClientValidation

Aplicación web ASP.NET Core 8.0 para validación de clientes con endpoints de ping y descarga de archivos.

## 🚀 Características

- **Ping Endpoint**: `/ping` - Responde con "Pong" para verificar que la aplicación está funcionando
- **Download Endpoint**: `/download` - Descarga el archivo `whitelist.txt`
- **Multiplataforma**: Configurado para desarrollo local, Docker e IIS
- **Docker Compose** listo para producción
- **Configuración optimizada** para JetBrains Rider

## 📋 Prerrequisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [JetBrains Rider](https://www.jetbrains.com/rider/) (opcional pero recomendado)

## 🚀 Inicio Rápido

### Desarrollo Local

1. Clona el repositorio
2. Navega al directorio del proyecto
3. Ejecuta:
   ```bash
   dotnet run
   ```
4. Abre http://localhost:8080/ping en tu navegador

### Usando Docker

#### Construir y ejecutar
```bash
docker-compose up -d --build
```

#### Ver logs
```bash
docker-compose logs -f
```

#### Detener
```bash
docker-compose down
```

## 🔧 Configuración

### Variables de Entorno

| Variable | Valor por defecto | Descripción |
|----------|-------------------|-------------|
| `ASPNETCORE_ENVIRONMENT` | `Production`      | Entorno de ejecución (Development/Production) |
| `DOTNET_ENVIRONMENT` | `Production`      | Entorno .NET |
| `ASPNETCORE_URLS` | `http://+:8080`   | URLs donde escucha la aplicación (dentro del contenedor) |

### Puertos

- **8080**: Puerto expuesto en el host para acceder a la aplicación
- **80**: Puerto interno del contenedor (no expuesto directamente)

## 🏗️ Estructura del Proyecto

```
ClientValidation/
├── Dockerfile           # Configuración de Docker
├── docker-compose.yml   # Orquestación de contenedores
├── Program.cs          # Punto de entrada de la aplicación
├── web.config          # Configuración para IIS
└── Resources/          # Archivos de recursos (whitelist.txt)
```

## 🐛 Solución de Problemas

### La aplicación no responde
- Verifica que el puerto 8080 no esté en uso
- Revisa los logs del contenedor: `docker-compose logs -f`

### Problemas con la carpeta Resources
- Asegúrate de que la carpeta `Resources` exista y contenga los archivos necesarios
- Verifica los permisos de la carpeta en producción

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

## 🏃‍♂️ Cómo Levantar el Proyecto

### Método 1: Docker Compose - Linux (Recomendado para desarrollo)

1. **Clonar el repositorio**
```bash
git clone <repository-url>
cd ClientValidation
```

2. **Levantar con Docker Compose**
```bash
# Puerto por defecto (8080)
docker-compose up --build -d

# Puerto personalizado
PORT=9000 docker-compose up --build -d
```

3. **Verificar que esté funcionando**
```bash
# Ver contenedores ejecutándose
docker ps

# Probar el ping
curl http://localhost:8080/ping
# o
Invoke-WebRequest -Uri http://localhost:8080/ping
```

### Método 2: Ejecución Directa con .NET

1. **Restaurar dependencias**
```bash
dotnet restore
```

2. **Ejecutar la aplicación**
```bash
# Puerto por defecto (8080)
dotnet run

# Puerto personalizado
ASPNETCORE_URLS=http://localhost:9000 dotnet run
```

### Método 3: Despliegue en IIS (Producción Windows)

1. **Publicar la aplicación:**
```bash
dotnet publish -c Release -o ./publish
```

2. **Configurar en IIS:**
   - Crear un nuevo sitio web
   - Establecer el directorio físico a la carpeta `./publish`
   - Configurar el pool de aplicaciones con .NET CLR versión "Sin código administrado"
   - Asegurarse de que la identidad del pool tenga permisos de lectura en la carpeta

3. **Verificar funcionamiento:**
```bash
# Probar el endpoint de ping
curl http://localhost/ping
```

### Método 4: Con JetBrains Rider

1. **Abrir el proyecto** en Rider
2. **Seleccionar perfil de ejecución**:
   - `ClientValidation`: Ejecución directa
   - `Docker`: Ejecución con Docker
   - `Docker Compose`: Ejecución con Docker Compose Linux (Recomendado para desarrollo)
   - `Docker Compose (IIS)`: Ejecución con Docker Compose IIS (Para producción)
3. **Presionar F5** o hacer clic en el botón de ejecutar

## 🔧 Configuración de Desarrollo

### Diferencias entre Linux e IIS

| Aspecto | Linux (Kestrel) | Windows (IIS) |
|---------|-----------------|---------------|
| **Imagen base** | `mcr.microsoft.com/dotnet/aspnet:8.0` | `mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver` |
| **Puerto por defecto** | 8080 | 80 |
| **Configuración** | Variables de entorno | web.config + Variables |
| **Servidor web** | Kestrel | IIS |
| **Rendimiento** | Más rápido | Más estable para Windows |
| **Uso recomendado** | Desarrollo | Producción Windows |

### Estructura del Proyecto
```
ClientValidation/
├── Program.cs                 # Punto de entrada de la aplicación
├── ClientValidation.csproj    # Archivo de proyecto
├── Dockerfile                # Configuración de Docker (Linux)
├── Dockerfile.iis            # Configuración de Docker (IIS/Windows)
├── docker-compose.yml        # Configuración de Docker Compose (Linux)
├── docker-compose.iis.yml    # Configuración de Docker Compose (IIS)
├── web.config                # Configuración para IIS
├── Properties/
│   └── launchSettings.json   # Configuración para Rider
├── Resources/
│   └── whitelist.txt         # Archivo de recursos
└── README.md                 # Este archivo
```

### Variables de Entorno Disponibles

| Variable | Descripción | Valor por Defecto |
|----------|-------------|-------------------|
| `PORT` | Puerto de la aplicación | `8080` |
| `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución | `Development` |
| `ASPNETCORE_URLS` | URLs donde escucha la aplicación | `http://+:8080` |

## 🌐 Endpoints Disponibles

### GET /ping
Verifica que la aplicación esté funcionando.

**Respuesta:**
```
Status: 200 OK
Content: "Pong"
```

**Ejemplo:**
```bash
curl http://localhost:8080/ping
```

### GET /download
Descarga el archivo `whitelist.txt`.

**Respuesta:**
```
Status: 200 OK
Content-Type: text/plain
Content-Disposition: attachment; filename=whitelist.txt
```

**Ejemplo:**
```bash
curl http://localhost:8080/download -o whitelist.txt
```

## 🐳 Comandos Docker Útiles

### Desarrollo
```bash
# Construir y levantar
docker-compose up --build -d

# Ver logs
docker-compose logs -f

# Detener servicios
docker-compose down
```

### Producción
```bash
# Reconstruir imagen
docker-compose build --no-cache

# Iniciar en producción
docker-compose up -d

# Ejecutar comando en el contenedor
docker-compose exec clientvalidation bash
```

### IIS
Para producción en IIS, simplemente publica la aplicación usando:
```bash
dotnet publish -c Release -o ./publish
```
Luego configura el sitio en IIS apuntando a la carpeta `publish`.

## 🐛 Troubleshooting

### Puerto ya en uso
Si el puerto 8080 está ocupado:
```bash
# Cambiar a otro puerto
PORT=9000 docker-compose up -d

# Ver qué está usando el puerto
netstat -an | findstr :8080
```

### Problemas con Docker
```bash
# Limpiar contenedores e imágenes
docker-compose down --rmi all --volumes --remove-orphans

# Verificar que Docker esté ejecutándose
docker --version
```

### Problemas con Rider
1. **Reiniciar Rider** para que lea los nuevos archivos de configuración
2. **Verificar que el perfil correcto esté seleccionado** (Docker Compose recomendado)
3. **Revisar los logs** en la ventana de Docker de Rider

## 📝 Logs

Los logs de la aplicación se muestran en la consola. Para ver logs detallados:

```bash
# Logs de Docker Compose
docker-compose logs -f clientvalidation

# Logs del contenedor directamente
docker logs clientvalidation-clientvalidation-1 -f
```

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.
