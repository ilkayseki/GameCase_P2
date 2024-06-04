# Project Title: Platform Walking Game

## Project Description
This project is a game where a character walks from a starting platform to an ending platform, based on user interaction and platform creation mechanics. The main mechanic of the game is the stabilization of moving platforms and the character's progression on these platforms.

## Requirements
This project has been developed according to the following requirements:

### General Requirements
- **Unity Version:** The project has been developed using either Unity 2020.3.x or 2021.3.x versions.
- **Version Control:** The project has been versioned with Git from the beginning and regular commits have been made.
- **Public Repository:** The project has been uploaded to a public platform like GitHub or Bitbucket.
- **SOLID Principles:** The code is written according to SOLID principles.
- **Dependency Injection:** Any Dependency Injection framework has been used (BONUS).
- **Code Quality:** The code is readable and follows a specific coding convention.

### Project Requirements
1. **Character Movement:** The character walks from the starting platform to the ending platform.
2. **Platform Movement:** The platforms to be walked on should come from the left and right, passing in front of the platform where the character is located.
3. **Platform Stabilization:** When the player touches the screen, the moving platform should be fixed in place, and the portions of the previous platform that extend beyond it should be cut and fall, then the next platform should start moving.
4. **Sound Effect:** If a perfect timing cut does not occur within a certain tolerance, a note sound should be played.
5. **Sound Pitch Increase:** With consecutive perfect timing cuts, this note should become progressively higher in pitch; when the perfect streak is broken, the note should return to its original pitch.
6. **Character Alignment:** The character should always move forward centered on the platform being created in front of it, adjusting to the left or right.
7. **Game Over - Failure:** If a platform cannot be created and the character falls, the game should end in failure.
8. **Game Over - Success:** If the character reaches the target, the game should end successfully.
9. **Celebration Animation:** When the player successfully completes a level, the character should perform a celebration animation on the final platform, and the camera should rotate around the character during this time (BONUS).
10. **Camera Usage:** Cinemachine should be used for the camera (BONUS).
11. **Level Transition:** After completing a level, the platforms for the new level should be appended to the end of the final platform of the previous level, and the scene should not be reloaded from scratch (BONUS).

## Technologies Used
- Unity 2021.3.x
- C#
- Git
- Zenject
- Textmesh Pro
- Cinemachine (BONUS)
