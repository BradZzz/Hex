using System;

[Serializable]
public class HighScoreInfo {
	
	//Level Name
	public string levelName;
	//Non-AI player
  public EndInfo[] scores;

  public static EndInfo makeEndInfo(GameInfo info){
    EndInfo end = new EndInfo ();
    end.rations = info.rations;
    end.gold = info.gold;
    end.rep = info.reputation;
    end.enemies = info.enemiesDefeated;
    end.squad = 0;
    foreach(UnitInfo unit in info.playerRoster){
      switch(unit.type){
      case UnitInfo.unitType.Knight:
        end.squad += 1;
        break;
      case UnitInfo.unitType.Lancer:
        end.squad += 1.2f;
        break;
      case UnitInfo.unitType.Swordsman:
        end.squad += .8f;
        break;
      case UnitInfo.unitType.Monster:
        end.squad += 2;
        break;
      }
    }
    end.attributes = info.attributes.Length;
    end.score = calculateScore(end);

    return end;
  }

  static float calculateScore(EndInfo end){
    return end.getRationsScore() + end.getGoldScore() + end.getRepScore() + end.enemies + end.squad + end.attributes;
  }

  public static string returnGrade(float score){
    if(score < 10) return "A+";
    if(score < 20) return "A";
    if(score < 30) return "A-";
    if(score < 40) return "B+";
    if(score < 50) return "B";
    if(score < 60) return "B-";
    if(score < 70) return "C+";
    if(score < 80) return "C";
    if(score < 90) return "C-";
    if(score < 100) return "D+";
    if(score < 110) return "D";
    if(score < 120) return "D-";
    return "F";
  }

  public static string returnRank(float score){
    if(score < 10) return "Siberian God";
    if(score < 20) return "Siberian Master";
    if(score < 30) return "Siberian Guru";
    if(score < 40) return "Animal Enforcer";
    if(score < 50) return "Animal Sensei";
    if(score < 60) return "Animal Caretaker";
    if(score < 70) return "Beast Noble";
    if(score < 80) return "Beast Peasant";
    if(score < 90) return "Karate Kitten";
    if(score < 100) return "Gentle Musker";
    if(score < 110) return "Puppy Whisperer";
    if(score < 120) return "Limp Poodle";
    return "Pitiful";
  }

  public class EndInfo {
    public int rations;
    public int gold;
    public int rep;
    public float enemies;
    public float squad;
    public float attributes;
    public float score;

    public int getRationsScore(){
      return rations > 250 ? 10 : 0;
    }
    public int getGoldScore(){
      return gold > 150 ? 10 : 0;
    }
    public int getRepScore(){
      return rep > 100 ? 10 : 0;
    }
  }
}
