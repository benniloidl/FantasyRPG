# Fantasy RPG Feature Development Project

## Objective & Instructions:

Continue developing the Fantasy RPG game created during previous assignments by adding new features to enhance its functionality. This project aims to improve your programming skills, practice design patterns, and understand system architecture by implementing at least **one (1)** of the following features.

> [!NOTE]
> You may choose to complete more than **one (1)** of the following features, earning additional points for each implemented feature.
> Each feature can earn a maximum of **10 points (30 points in total)** based on the evaluation criteria, which assesses functionality, design quality, code quality, user experience, and documentation.

---

### Feature Options

#### 1. Database Integration (Max: 10 Points)

- **Objective**: Create a new relational database for the FantasyRPG game using SQL. You may choose any relational database management system.
- **Requirements**:
    - The database should store the following data:
        - **Character**:
            - Class-specific attribute values (health, mana, etc.)
            - Created characters (e.g., from previous game instances)
            - Unique characters: "Static" characters whose information remains consistent across game instances
        - **Enemy**:
            - Enemy-type specific attribute values (health, mana, etc.)
            - Created enemies (e.g., from previous game instances)
            - Unique enemies: "Static" characters with consistent information across instances
        - **Item**:
            - Item creation data (e.g., weapon types, base damage values)
            - Unique items: "Static" items with consistent attributes across instances
        - **Character Equipments/Items**:
            - Equipments for player characters, enemy characters, and NPCs (e.g., merchant inventories)
        - **World Data**:
            - Data for world generation (e.g., locations, biomes, tile types, sizes)
            - Gameworld data from previous instances

#### 2. Enhanced Gameworld Generation and Interaction (Max: 10 Points)

- **Objective**: Expand the game world generation to include new locations, NPC types, quests, and an interactive gameworld system.
- **Requirements**:
    - **Gameworld Generation**:
        - New locations, including Villages, Towns, and Dungeons
        - Generate NPCs for each location, with types including Villager, Merchant, King/Queen
    - **Quests**:
        - Generate quests that can be assigned to NPCs or triggered by events (e.g., combat in specific locations)
        - Quests may include rewards (e.g., gold, items)
    - **Gameworld Interaction**:
        - Create a system allowing players to move around in the world and interact with NPCs
        - Allow interactions like accepting and completing quests, and engaging in dialogue

#### 3. Combat Manager (Max: 10 Points)

- **Objective**: Implement a turn-based combat manager to simulate and manage combat between player characters and enemies.
- **Requirements**:
    - **Turn-Based Combat**:
        - Manage turn-based combat, allowing each character or enemy to perform an action per turn
        - Enable character targeting, including changing targets during combat
        - Implement a simple enemy AI (e.g., random actions and target selection, can use random number generators to simulate AI)
    - **Equipment-Based Calculations**:
        - Include equipped items in damage and defense calculations for characters and enemies
        - Both players and enemies can equip items, affecting total action value calculations
        - Defensive items can reduce total damage taken from attacks
    - **Healing Actions**:
        - Both players and enemies can use healing actions, either on themselves or others
    - **Downed State**:
        - Characters or enemies can be downed in combat and cannot be targeted until revived
        - Downed characters can be healed/revived by allies
    - **End of Combat**:
        - Combat ends when all player characters or all enemies are defeated

---

## Evaluation and Grading

Each feature implementation will be evaluated on the following criteria:

### 1. Functionality and Correctness (0-2 Points)

- The feature fully meets the specified requirements and objectives.
- All components work as intended, without critical bugs or errors.
- Edge cases and invalid inputs are handled gracefully.

  **Evaluation Examples**:
  - **0 points**: Feature implementation doesn’t meet the requirements or is missing critical parts.
  - **1 point**: Implementation meets requirements but lacks handling for edge cases and potential bugs.
  - **2 points**: Implementation fully meets requirements and handles edge cases and errors effectively.

### 2. Design and Architecture (0-2 Points)

- Appropriate design patterns are identified and correctly implemented.
- The feature demonstrates good architectural principles, such as modularity and separation of concerns.
- The design supports scalability and integration with other system components.

  **Evaluation Examples**:
  - **0 points**: Implementation does not utilize appropriate design patterns.
  - **1 point**: Implementation uses appropriate design patterns but has scalability or maintainability issues.
  - **2 points**: Implementation uses suitable design patterns with consideration for scalability and maintainability.

### 3. Code Quality and Readability (0-2 Points)

- Code follows consistent coding standards and best practices.
- Variables, functions, and classes are well-named and self-explanatory.
- The code is well-organized with a logical structure and flow.

  **Evaluation Examples**:
  - **0 points**: Code is messy, hard to read, and lacks consistent standards.
  - **1 point**: Code is readable and follows some standards but lacks clear, maintainable structure.
  - **2 points**: Code is readable, follows standards, and is well-organized with a logical structure.

### 4. User Experience and Interaction (0-2 Points)

- The feature provides a clear and intuitive user interface or interaction model.
- Feedback to the user is informative and enhances usability.
- The user experience aligns with the overall design of the project.

  **Evaluation Examples**:
  - **0 points**: Implementation lacks a user interface or interaction model.
  - **1 point**: Basic user interface is included but lacks sufficient or clear feedback.
  - **2 points**: UI/interaction model is intuitive, with necessary feedback for the user.

### 5. Documentation and Comments (0-2 Points)

- Code is adequately commented, explaining complex logic or important decisions.
- User-facing documentation explains how to use the feature.
- Technical documentation outlines the feature’s implementation and system integration.

  **Evaluation Examples**:
  - **0 points**: Code lacks comments, and documentation is unclear or missing.
  - **1 point**: Basic comments and documentation exist but lack clarity and detail.
  - **2 points**: Code is well-commented, with clear user and technical documentation.


