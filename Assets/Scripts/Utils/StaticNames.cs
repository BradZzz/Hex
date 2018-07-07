using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticNames {

  public static string[] prefix = new string[] {
    "Fort", "Great", "New", "Noble", "Los", "Flowering", "Gilded", 
    "Glorious", "Withering", "Decaying", "Old", "Plagued", "Lawless"
  };

  public static string[] name = new string[] {
    "Adalexine", "Chors", "Flanneid", "Nazic",
    "Aducuston", "Ciacus",  "Galatur", "Neston",
    "Aegelia", "Clour", "Gilpinn", "Otgilmerth",
    "Ainnaci", "Colaid",  "Gimmongal", "Quingus",
    "Akmirin", "Columatach",  "Gobia", "Rammore",
    "Alcuin", "Corment", "Halogius", "Regolindi",
    "Aldegill", "Corwarda", "Hayline", "Restere",
    "Allorice", "Credaimid", "Hitte", "Rutius",
    "Anandirien", "Curmund", "Isorreytel", "Samesica",
    "Angana",  "D\'Aufus", "Ivanah",  "Sasins",
    "Anorabenie",  "Dallonus", "Jaxos", "Seulte",
    "Arinamia",  "Davir", "Jenni", "Sitigheren",
    "Arleirdan", "Derwyth", "Kelvero", "Somerick",
    "Arlow", "Duwan", "Knowaine", "Stremost",
    "Arthon", "Duwulf", "Lauriel", "Suana",
    "Arzel", "Eadwan", "Leffinka", "Thanna",
    "Audrudy", "Ecchel", "Lunak", "Thoks",
    "Barnes", "Echana", "Mailo", "Tiarlin",
    "Bearust", "Edmon", "Maladhrond", "Tiffen",
    "Bloch", "Erank", "Manus", "Tristeinar",
    "Boyna", "Fabled", "Menanne", "Vanbenea",
    "Braetman", "Faeron", "Meresari", "Veolasarok",
    "Bryth", "Fanus", "Merioght", "Xercus",
    "Caciaring", "Fastanui", "Merosef", "Zakayt",
    "Celain", "Fietu", "Mohan", "Zanna",
    "Alena", "Bytlinn", "Gwenvellen", "Morga",
    "Ambioca", "Caerong", "Gwyann",  "Morix",
    "Amric", "Caleeke", "Hauses",  "Nolan",
    "Anbhachael",  "Callon",  "Heremile",  "Oderec",
    "Arave", "Camund", "Hjalanord", "Padre",
    "Arlan", "Carana",  "Inibile", "Padrindur",
    "Artherin",  "Crain", "Ionna", "Poignyerin",
    "Audagunzey",  "Croven",  "Isont", "Raengelm",
    "Awenderbo", "Damrysey",  "Iteset",  "Ragorseis",
    "Babelagh",  "Eborn", "Josefarel", "Randui",
    "Bairin",  "Edhil", "Joyciyf", "Reochar",
    "Baldis",  "Eksiz", "Kevel", "Samzun",
    "Bavor", "Eljin", "Kylin", "Sansulinn",
    "Bearana", "Esteige", "Leven", "Sesan",
    "Belon", "Faren", "Lithet", "Slefleace",
    "Beran", "Fionall", "Maelasee", "Snach",
    "Bergille",  "Firrah", "Magand", "Stein",
    "Bjorg", "Fridariann", "Mated", "Strall",
    "Blamanna", "Fronwy", "Matto", "Trifier",
    "Boswayne", "Gartine", "Maynan", "Tuire",
    "Brullig", "Gillus", "Melia", "Varank",
    "Brunters", "Grunt", "Melisance", "Wathgaman",
    "Brych", "Guntinyi",  "Meregonthe",  "Whilar",
    "Brynawd", "Gupperede", "Moirine", "Wilhel",
    "Bucius", "Gwendy", "Moreth", "Wyvarin"

  };

  public static string[] suffix = new string[] {
    "Town", "Duchy", "Market", "Bazaar", "City", "Outpost"
  };

  public static string getTownName(){
    string pre = Random.Range (0, 10) > 1 ? "" : (prefix[Random.Range (0, prefix.Length)] + " ");
    string suf = Random.Range (0, 20) > 1 ? "" : (" " + suffix[Random.Range (0, suffix.Length)]);
    return pre + name [Random.Range (0, name.Length)] + suf;
  }
}
