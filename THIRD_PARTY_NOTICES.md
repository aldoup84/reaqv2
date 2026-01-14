# Third-Party Notices

This repository is licensed under GPL-3.0-or-later **only** for:
- Original source code authored by the project maintainers, and
- Original assets created by the project maintainers,
unless a file explicitly states otherwise.

Third-party assets (including Unity Asset Store content) are **NOT** covered by GPL-3.0-or-later and remain under their respective terms.

## Unity / Engine
Unity® is a trademark of Unity Technologies. Unity and its components are governed by Unity’s own license terms.

---

## Unity Asset Store Content
This project uses assets obtained from the Unity Asset Store. These assets are licensed under the **Standard Unity Asset Store EULA** and/or publisher-specific terms.

The following assets are NOT covered by GPL-3.0-or-later:

1) **Camera Shake FX**
   - Source: Unity Asset Store
   - License: Standard Unity Asset Store EULA
   - Notes/Restrictions: Redistribution of the asset source files may be restricted under the EULA.

2) **KY Magic Effects**
   - Source: Unity Asset Store
   - License: Standard Unity Asset Store EULA

3) **Splendid Explosion and Smoke Effects**
   - Source: Unity Asset Store
   - License: Standard Unity Asset Store EULA

4) **White Smoke Particle System**
   - Source: Unity Asset Store
   - License: Standard Unity Asset Store EULA

---

## Third-Party Libraries / Services

### Vuforia Engine (used in ReAQ v2)
- Version: **10.25.4**
- Provider: PTC / Vuforia
- Notes: Vuforia is installed via Unity Package Manager. Due to GitHub file size limits, the package file
  `Packages/com.ptc.vuforia.engine-*.tgz` may not be included in this repository. You may need to install the
  required Vuforia version manually in Unity (Package Manager) as documented in `README.md`.
- License/Terms: Vuforia license/terms apply (see vendor documentation/terms).

### Firebase Unity SDK (used in ReAQ v2)
- Version: **12.2.1**
- Provider: Google / Firebase
- Notes: Firebase may **not** be included in this repository and may need to be imported manually via `.unitypackage`.
  After import, run the External Dependency Manager (resolver).
- License: Firebase SDK components are licensed under the Apache License 2.0.
  If the Apache 2.0 text is included in this repository, it is provided at `licenses/APACHE-2.0.txt`.
- Service Terms: Use of Firebase services is additionally governed by Google/Firebase Terms of Service.

### SQLite (used in ReAQ v1)
SQLite is **public domain** (no license required), but may include optional purchase/license documents for warranty of title.

---

## Build / Redistribution Notes
Some third-party assets and SDKs may not be included in this repository due to license restrictions or file size limits.
To build the full project from source, you may need to obtain and import/install the required third-party content listed above.
