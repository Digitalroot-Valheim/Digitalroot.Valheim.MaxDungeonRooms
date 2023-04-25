## Summary
Raises the chance to spawn larger dungeons. 

## License
**GNU Affero General Public License v3.0**

# Digitalroot's Max Dungeon Rooms
Raises the change to spawn larger dungeons. Valheim makes a limited
number of attempts to spawn rooms for a dungeon.

The defaults for **Forest Crypt** are _20/40_.  
The defaults for **Sunken Crypt** are _20/30_.

This mod alters these values for a higher chance of spawning more rooms
per dungeon. 

### FAQ
 - What is the lowest number of rooms I have spawned using this mod? **6**
 - What is the highest number of rooms I have spawned using this mod? **99**
 - Will this cause performance issue? **Yes, it can on slower PCs.**
 - What happens to spawned dungeons if the mod is removed? **They stay, they are part of the world save file.**

### The Algorithm

The _Min/Max_ values do not functions as I expected them to. 
The algorithm the **Dungeon Generator** uses looks like the following.

```c#
for (int i = 0; i < maxRooms; i++)
{
  TryToPlaceOneRoom();
  if (HasAllRequiredRooms() && placedRooms.Count > minRooms)
  {
    break;
  }
}
```
**Dungeon Generator** will stop spawning more rooms when either of the two statement are true.
 - All required rooms have spawned and the _min_ number of rooms have spawned too.
 - **Dungeon Generator** has made up to the _max_ number of attempts to spawn rooms.

### Installation (manual)
1. Download the latest release archive (zip) file.
1. Extract the archive into &lt;Steam Location&gt;\steamapps\common\Valheim\BepInEx\plugins

### Configuration 
- Min Rooms
- Max Rooms

##### Advanced Configuration 

> Each dungeon type can have their own values. Enable the override for the dungeon and set it's values. 
> If an override is not enable, then the values from the __General__ section are used.


### Issues/Questions/Bugs
- Use the repo issues tab above.

### Support Me @ https://www.buymeacoffee.com/digitalroot
<br />
<p align="center">
<b>Digitalroot can be found in the Valhalla Legends Discord</b><br /><br />
  <a href="https://discord.gg/SsMW3rm67u" target="_blank"><img src="https://digitalroot.net/img/vl/vl_logo_125x154.png"></a>
</p>
