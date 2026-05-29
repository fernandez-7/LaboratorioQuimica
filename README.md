# 🧪 Digital Twin de Laboratorio de Química
### I.E.P Latino — Educación de Calidad con IA

Laboratorio de química virtual interactivo desarrollado en Unity, diseñado para estudiantes de secundaria que no cuentan con acceso a un laboratorio físico.

---

## 📋 Descripción del Problema

En el **Colegio Privado Latino**, la enseñanza de Ciencias es principalmente teórica, basada en libros y pizarras. La falta de un laboratorio físico impide realizar experimentos reales, obligando a los estudiantes a imaginar los procesos y dificultando su comprensión. Como resultado, se genera desinterés, dudas y desventaja frente a otros colegios.

**Digital Twin de Laboratorio** resuelve este problema creando un laboratorio virtual inmersivo donde los estudiantes pueden:
- 🔬 Explorar un laboratorio equipado en primera y tercera persona
- ⚗️ Interactuar con instrumentos de química reales (frascos, pipetas, microscopios)
- 🧑‍🔬 Seleccionar un avatar personalizado (niño o niña)
- 🎮 Aprender de forma práctica y motivadora

---

## 🖼️ Capturas del Proyecto

### Menú Principal
![Menú Principal](Capturas/Menu%20Principal.png)

### Selección de Personaje
![Selección de Personaje](Capturas/Seleccion%20de%20Personajes.png)

### Laboratorio Principal
![Laboratorio](Capturas/Laboratorio%20Principal.png)

---

## 🛠️ Tecnologías Utilizadas

| Tecnología | Versión | Uso |
|---|---|---|
| **Unity** | 2022.3.62f1 LTS | Motor de videojuego |
| **C#** | — | Lenguaje de programación |
| **Mixamo (Adobe)** | — | Personajes y animaciones 3D |
| **TextMeshPro** | — | UI y textos del juego |
| **Input System** | 1.14.2 | Control de teclado y mouse |
| **GitHub** | — | Control de versiones |

### Assets utilizados
- 🧪 **Free Laboratory Pack** — Unity Asset Store (instrumentos de vidrio)
- 🔬 **Microscopio 3D** — Poly.pizza (modelo OBJ gratuito)
- 📊 **Tabla Periódica** — Imagen PNG en español

---

## 💻 Requisitos de Instalación

### Requisitos mínimos del sistema
- **OS:** Windows 10 / 11 (64-bit)
- **RAM:** 8 GB mínimo
- **GPU:** Tarjeta gráfica con soporte DirectX 11
- **Almacenamiento:** 2 GB libres

### Para desarrolladores (editar el proyecto)
- **Unity Hub** instalado
- **Unity 2022.3 LTS** (versión exacta: 2022.3.62f1)
- **Visual Studio Code** o Visual Studio 2022
- **Git** instalado

---

## 🚀 Cómo Ejecutar

### Opción A — Ejecutar el juego compilado
```
1. Descarga la carpeta Build/ del repositorio
2. Ejecuta LaboratorioQuimica.exe
3. El juego inicia automáticamente desde el Menú Principal
```

### Opción B — Abrir en Unity Editor
```
1. Clona el repositorio:
   git clone https://github.com/fernandez-7/LaboratorioQuimica

2. Abre Unity Hub → Add → selecciona la carpeta clonada

3. Asegúrate de tener Unity 2022.3 LTS instalado

4. Abre la escena: Assets/Scenes/MenuPrincipal.unity

5. Presiona ▶ Play
```

### 🎮 Controles del juego
| Tecla | Acción |
|---|---|
| `W A S D` | Moverse |
| `Shift` | Correr |
| `Mouse` | Mirar alrededor |
| `Q` | Cambiar entre 1ra y 3ra persona |
| `Escape` | Liberar cursor |

---

## 📁 Estructura del Proyecto

```
Assets/
├── Audio/
├── Characters/          # Personajes Timmy y Amy (Mixamo)
│   ├── BoyTextures/
│   ├── GirlTextures/
│   └── PersonajeAnimator.controller
├── Materials/
├── Models/
│   └── Microscopio/
├── Prefabs/
├── Scenes/
│   ├── MenuPrincipal.unity
│   ├── SeleccionPersonaje.unity
│   └── Laboratorio_Principal.unity
├── Scripts/
│   ├── MenuManager.cs
│   ├── SeleccionPersonaje.cs
│   ├── ControladorPersonaje.cs
│   └── SpawnManager.cs
├── Textures/
└── ThirdParty/
    └── FreeLabAssets/
```

---

## ✅ Estado del Proyecto

> 🚧 **Versión actual: 60% completado**

| Fase | Descripción | Estado |
|---|---|---|
| **Fase 1** | Laboratorio visual (escenario, mobiliario, instrumentos) | ✅ Completo |
| **Fase 2** | Menú principal, selección de personaje, sistema de cámaras | ✅ Completo |
| **Fase 3** | Sistema de interacción con objetos | 🔄 En desarrollo |
| **Fase 4** | Reacciones químicas con efectos visuales | ⏳ Pendiente |

---

## 👥 Equipo de Desarrollo

| Nombre | Rol |
|---|---|
| **Fernández Chuse, Abel** | 💻 Programador Principal |
| **Chuquimango Cueva, José** | 🎨 Diseñador de Escenario 3D |
| **Levano Villanueva, Gianfranco** | 🎮 Diseñador de Experiencia de Usuario (UX) |
| **Lozano Salazar, Ángel** | 🔬 Investigador de Contenido Educativo |
| **Pretell Calderón, Luis** | 🧪 Tester y Control de Calidad |
| **Villalobos Acuña, Briseth** | 📋 Gestora de Proyecto y Documentación |

---

## 🎬 Video Demo

> 🎥 **Próximamente** — El video demo será publicado al completar el 100% del proyecto.

---

## 📌 Notas Técnicas

- **NO actualizar** Unity a versiones superiores a 2022.3 LTS
- **NUNCA** mover archivos desde el Explorador de Windows — siempre usar el Panel Project de Unity
- Hacer **commit en GitHub** después de cada sesión de trabajo
- La escena principal de inicio es `MenuPrincipal`

---

<div align="center">
  <strong>🧪 Digital Twin de Laboratorio de Química</strong><br>
  Desarrollado con Unity 2022.3 LTS · C# · Mixamo · GitHub<br><br>
  <em>"Educación de Calidad"</em>
</div>
