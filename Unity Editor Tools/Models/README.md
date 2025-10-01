
### FBXExtractor

A utility tool that automatically splits a selected `.fbx` file into clean, reusable assets — **separating meshes**, **materials**, and **textures** into organized folders and generating **individual prefabs**.

Ideal for working with **purchased asset packs**, **model kits**, or **external 3D files** where you want editable, Unity-native components.

#### Features

- Auto-extracts mesh objects into individual prefabs.
- Separates and relinks all materials.
- Extracts and deduplicates textures used in the model.
- Organizes assets into:
  - `/Models` – Mesh prefabs
  - `/Materials` – Extracted materials
  - `/Textures` – Shared textures (1 copy per texture)
- Fully preserves material-texture-mesh connections.

### Usage

1. **Place the Script**  
   Save `FBXExtractor.cs` in your project under:  
   `Assets/Editor/FBXExtractor.cs`

2. **Select an FBX Model**  
   In the Project view, click on any `.fbx` file (not a prefab instance).

3. **Run the Extractor**  
   In the top menu bar, go to:  
   `Tools > FBX > Extract Full Contents`

4. **Output**  
   A new folder structure is created next to your FBX:
   ```
   ├── MyModel.fbx
   ├── Models/
   ├── Materials/
   └── Textures/
   ```

---

### DAEExtractor

Same as `FBXExtractor`, but designed for `.dae` (COLLADA) format files.  
Useful for importing content from older 3D pipelines or tools that only export `.dae`.

#### Features

- Identical functionality to the FBX extractor.
- Supports `.dae` files imported into Unity.
- Automatically reconstructs textures and materials.
- Produces clean Unity-native prefabs for each mesh.

### Usage

1. **Place the Script**  
   Save `DAEExtractor.cs` in your project under:  
   `Assets/Editor/DAEExtractor.cs`

2. **Select a DAE Model**  
   In the Project view, click on any `.dae` file.

3. **Run the Extractor**  
   In the top menu bar, go to:  
   `Tools > DAE > Extract Full Contents`

4. **Output**  
   A new folder structure is created next to your DAE:
   ```
   ├── MyModel.dae
   ├── Models/
   ├── Materials/
   └── Textures/
   ```
