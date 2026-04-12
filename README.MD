## AnyMovie Adder 🎬

A small application for adding movies and TV shows to my Anytype space via API, using [Anytype.NET](https://github.com/glabular/Anytype.NET) nuget package.

This is a personal learning project and is tightly coupled to my own Anytype setup.
It is not intended to be used as-is, it will not work out of the box for other users.

Feel free to explore the code, reuse ideas, or adapt parts of it for your own projects.

### How it is structured

- **AnyMovieAdder.Host** — Windows desktop shell (WinForms + WebView2). Starts the web server in-process and opens the UI at `https://127.0.0.1:7185/`.
- **AnyMovieAdder.API** — ASP.NET Core API and static file host. The TypeScript UI is copied into `bin/.../wwwroot` at build time from **Frontend** (same origin as `/api`, so no hard-coded API URL in the client).
- **Frontend** — TypeScript sources; run `npx tsc` so `dist/` exists before `dotnet build`.

### Run from source

1. Install [.NET 10 SDK](https://dotnet.microsoft.com/download) and [Node.js](https://nodejs.org/) (for the frontend build).
2. Install the [WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/) if Windows does not already have it (most Windows 10/11 systems do).
3. Trust the ASP.NET Core HTTPS development certificate (needed for WebView2 to load `https://127.0.0.1:7185`):

   ```powershell
   dotnet dev-certs https --trust
   ```

4. Build the UI, then run the host (from the repository root):

   ```powershell
   cd Frontend
   npm install
   npx tsc
   cd ..
   dotnet run --project AnyMovieAdder.Host
   ```

To run only the API (for example debugging without the desktop window):

```powershell
dotnet run --project AnyMovieAdder.API --launch-profile https
```

### Publish (framework-dependent, single-file host)

From the repository root, after `npx tsc` in **Frontend**:

```powershell
dotnet publish AnyMovieAdder.Host\AnyMovieAdder.Host.csproj -c Release
```

Output is under `AnyMovieAdder.Host\bin\Release\net10.0-windows\win-x64\publish\`. The main entry is **AnyMovieAdder.Host.exe** (single-file). Copy the **`wwwroot`** folder next to that executable if it is not already there (the publish step should place it beside the exe).

The target machine needs the **.NET 10** runtime (desktop + ASP.NET) and the **WebView2** runtime; the build is not self-contained.

### Demo

![Add movie flow](assets/demo1.gif)
