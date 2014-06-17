using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace thegame
{
    class Language
    {

        static private string language_type;
        static public Dictionary<string, string> Text_Game;


        static public void change(string language)
        {
            language_type = language;
            switch (language_type)
            {
                case "english":
                    Text_Game = new Dictionary<string, string>()
                    {
                                                   {"_mnuPlay","Play"},
                                                   {"_mnuOptions","Options"}, 
                                                   {"_mnuMultiplayer","Multiplayer"}, 
                                                   {"_mnuExit","Exit"},
                                                   {"_mnuLanguage","Language"},
                                                   {"_mnuFullscreen","Full screen"}, 
                                                   {"_mnuBack","Back"},
                                                   {"_mnuEnglish","English"},
                                                   {"_mnuFrench","Francais"}, //has to be français
                                                   {"_mnuSound","Sound"}, 
                                                   {"_mnuOn","On"},
                                                   {"_mnuOff","Off"},
                                                   {"_mnuDutch","Nederlands"},
                                                   {"_gamePause","Press P to pause"},
                                                   {"_gameHelp","Press H to get help"},
                                                   {"_gameHealth","Health"},
                                                   {"_gamescore","score"},
                                                   {"_gamebonus","points"},
                                                   {"_gameHelpLine1","Use the left and right arrow to move the character"},
                                                   {"_gameHelpLine2","Use the top arrow to jump"},
                                                   {"_gameHelpLine3","To fire on ennemies use the space bar"},
                                                   {"_gameHelpLine4","Press any key to exit"},
                                                   {"congrats","End of the level, so badass..."},
                                                   {"finalScore","Ennemies Killed : "},
                                                   {"finalBonus","Bonus Score : "},
                                                   {"total","Total : "},
                                                   {"space","Press Space key"},
                                                   {"_btnPlay","Resume"},
                                                   {"_btnMenu","Menu"},
                                                   {"_btnQuit","Exit"},
                                                   {"player","Player"},
                    
                    };
                    break;

                case "french":
                    Text_Game = new Dictionary<string, string>()
                    {                              
                                                   {"_mnuPlay","Jouer"},
                                                   {"_mnuOptions","Options"},
                                                   {"_mnuMultiplayer","Multijoueur"}, 
                                                   {"_mnuExit","Quitter"},
                                                   {"_mnuLanguage","Langue"},
                                                   {"_mnuFullscreen","Plein ecran"}, 
                                                   {"_mnuBack","Retour"},
                                                   {"_mnuEnglish","English"},
                                                   {"_mnuFrench","Francais"},
                                                   {"_mnuSound","Son"},
                                                   {"_mnuOn","Actif"},
                                                   {"_mnuOff","Inactif"},
                                                   {"_mnuDutch","Nederlands"},
                                                   {"_gamePause","P = pause"},
                                                   {"_gameHelp","H = aide"},
                                                   {"_gameHealth","Vie"},
                                                   {"_gamescore","score"},
                                                   {"_gamebonus","points"},
                                                   {"_gameHelpLine1","Utilisez la flèche gauche/droite pour se déplacer"},
                                                   {"_gameHelpLine2","Utilisez la flèche du haut pour sauter"},
                                                   {"_gameHelpLine3","Utilisez la barre espace pour tirer"},
                                                   {"_gameHelpLine4","Appuyez sur une touche pour quitter"},
                                                   {"congrats","Fin du niveau, trop badass..."},
                                                   {"finalScore","Nombre d'ennemis morts : "},
                                                   {"finalBonus","Score bonus : "},
                                                   {"total","Total : "},
                                                   {"space","Appuyer sur la touche Espace"},
                                                   {"_btnPlay", "Reprendre"},
                                                   {"_btnMenu","Menu"},
                                                   {"_btnQuit","Quitter"},
                                                   {"player","Joueur"},
                    };
                    break;

                case "nederlands":
                    Text_Game = new Dictionary<string, string>()
                    {
                                                   {"_mnuPlay","Spelen"},
                                                   {"_mnuOptions","Opties"}, 
                                                   {"_mnuMultiplayer","Multijoueur"}, //TODO : translate this
                                                   {"_mnuExit","Afsluiten"}, 
                                                   {"_mnuLanguage","Taal"},
                                                   {"_mnuFullscreen","Volledig scherm"},
                                                   {"_mnuBack","Terug"},
                                                   {"_mnuEnglish","English"},
                                                   {"_mnuFrench","Francais"},
                                                   {"_mnuSound","Geluid"},
                                                   {"_mnuOn","Aan"},
                                                   {"_mnuOff","Uit"},
                                                   {"_mnuDutch","Nederlands"},
                                                   {"_gamePause","Druk P voor pause"},
                                                   {"_gameHelp","Druk H voor help"},
                                                   {"_gameHealth","Health"},
                                                   {"_gamescore","score"},
                                                   {"_gamebonus","bonus"},
                                                   {"_gameHelpLine1","Cursor links en rechts om te bewegen"},
                                                   {"_gameHelpLine2","Cursor omhoog om te springen"},
                                                   {"_gameHelpLine3","Spaciebalk om te schieten"},
                                                   {"_gameHelpLine4","Druk een willekeurige om werder te spelen"},
                                                   {"congrats","Geweldig! Je hebt het level gehaald"},
                                                   {"finalScore","Je score is : "},
                                                   {"finalBonus","Bonus score : "},
                                                   {"total","Totaal : "},
                                                   {"space","Druk spacie balk"},
                                                   {"_btnPlay","Hervatten"},
                                                   {"_btnMenu","Menu"},
                                                   {"_btnQuit","Afsluiten"},
                                                   {"player","Player"},
                    };
                    break;
            }
        }
    }
}
