using System;



namespace pendu_unity
{
    
    class Program
    {
    static string[] MotPendu = new string[] {"beau", "belle","terrible","jupe","paris"};//mot trouvable dans le jeu
    static bool viebonus=false;
    static bool IsApplicationOn=true;
    static string MotJoueur="";
    static string motcache = motjeu();//contient le mot choisi aléatoirement
    static char[]jeu=motcache.ToCharArray(); // tableau qui contient chaque lettre du mot choisi
    static char[]Lettretrouvé=new char[jeu.Length];//tableau qui affichera les lettres trouvé et non trouvé par le joueur
    static char[]essai=new char[10];//tableau qui va etre comparé au tableau jeu pour vérifier les lettres trouvé
    static string LettreJoueur="";
    static string choix;//string qui va choisir si on veut rejouer ou non
    static bool victoire=false;//défini si le jeu est gagné ou non
    static viejoueur vie=new viejoueur();//nombre de vie du joueur
    static int nbrVie;
    static char[] mauvaiselettre=new char[26];
    static string pendu;
    static string rejouer="";
    static int combo=0;
    static textePendu motif = new textePendu();
    static string test="";
    

    //
    static void Main(string[] args)//lance le jeu
    {     
        while(IsApplicationOn == true)//fait tourner le jeu tant qu'on décide de rejouer ou non
        {    
            start();//donne le nombre de vie
            
            
            titre();//affiche le titre du jeu
            Update();//fonction principale qui permet de jouer
        }
    }


//-------------------------------------------------------------------------------fonction vues
        
    static void titre(){
        Console.ForegroundColor=ConsoleColor.DarkBlue;
        Console.WriteLine("************** \n*jeu du pendu* \n************** ");//titre du jeu
        Console.ResetColor();
    }   

    static void message(string message,ref string actionjoueur){//envoie un message qui demande une action console.readline du joueur
        Console.ForegroundColor=ConsoleColor.Blue;
        Console.WriteLine(message);
        Console.ResetColor();
        actionjoueur=Console.ReadLine();
    }

    static void errormessage(string message){//message en cas d'erreur
         Console.ForegroundColor=ConsoleColor.Red;
        Console.WriteLine("Erreur: "+message);
        Console.ResetColor();
    }

    static void infomessage(string message){//message informatif sur l'avancé du jeu
        Console.ForegroundColor=ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    static string dessin(int viependu){//fonction qui permet de choisir le bon dessin du pendu a afficher 
        Console.ForegroundColor=ConsoleColor.Green;
        string pendu = motif.dessinpendu[viependu];
        return pendu;
    }
//--------------------------------------------------------------------------fonctions programme 
        static void start(){
            nbrVie=vie.vie;
            //test=vie.texte;
            //infomessage(test);
        }

        static void Update() {     
        //Console.WriteLine("le mot choisi est " +motcaché);//pour voir quel est le mot choisi (le mot sera caché pour les joueurs)
        for(int j =0;j<jeu.Length;j++){//crée la suite de tiret qui représente les lettres cachés qui seront ensuite découverte
                Lettretrouvé[j]=Convert.ToChar("-");
            }
            Console.WriteLine(Lettretrouvé);
            for(int j =0;j<mauvaiselettre.Length;j++){//crée la suite de tiret qui représente les lettres cachés qui seront ensuite découverte
                mauvaiselettre[j]=Convert.ToChar("/");
            }
        while(nbrVie>0){//boucle qui va fait jouer le joueur juste qu'a qu'il perde ou qu'il gagne
            message("voulez vous rentrer une lettre ? (taper 1) ou bien un mot ? (taper 2)",ref choix);
            if(choix=="1"){//-----si on choisit de taper une lettre---------
            Lettre();//fonction qui va permettre de joueur une lettre
            bonus();//fonction qui fait gagner un bonus de vie si lettre bonne consécutive
            }
            else if(choix=="2"){//--- si on choisit de taper un mot---------
            Mot();//fonction qui permet de jouer tout un mot d'un coup
            }
            else{
                errormessage("taper un nombre valide !");
                if(nbrVie<7){
                nbrVie=nbrVie+1;
                }
            }
            //------code qui vont vérifier si on a trouvé le mot------
            if(choix=="1"){
                victoire=victoire1();

            }
            else if(choix=="2"){//vérifie
                victoire=victoire2();
                
            }            
            if(victoire==true){//si on a gagné
            infomessage("bravo vous avez trouvé le mot mystère " + motcache+" vous gagnez");
            break;
            }
            else{//si on a pas gagné enlève une vie, on affiche les lettre trouvés , on indique le nombre de vie restante
                Console.WriteLine(Lettretrouvé);//affiche les lettres trouvées 
                infomessage("Plus que "+(nbrVie)+" essaie avant que le bonhomme sois pendu");
            }
        }
        if(victoire==true){//annonce si on a gagné ou perdu
            infomessage("bravo fin du jeu");
        }
        else{
            infomessage("dommage vous avez perdu. fin du jeu");
        }
        restart();//fonction qui permet de rejouer ou non
        }
            
    

        static string motjeu(){//fonction pour choisir un des mot pour le jeu aléatoirement

            int range=MotPendu.Length;
            int MotRandom=0;
            string motchoisi="";
            Random rnd = new Random();
            MotRandom = rnd.Next(range);
            motchoisi = MotPendu[MotRandom];
            return motchoisi;
        }
        // --------------FONCTIONS QUI VERIFIE LES LETTRES RENTREES---------------- 
        static char[] Mot(){ //fonction qui vérifie si chaque lettre est identique a celle du mot cherché
            message("entrez un mot de "+jeu.Length+" caractères",ref MotJoueur);
            char[] essai = MotJoueur.ToCharArray();
             if(essai.Length!=jeu.Length){
                errormessage("entrer un mot avec "+jeu.Length+" lettres");
                }
            if(essai.Length==jeu.Length){
            for(int p=0;p<jeu.Length;p++){ //vérifie chaque lettre et affiche celles qui sont justes
                if(essai[p]==jeu[p]){
                    Lettretrouvé[p]=(essai[p]);
                }
            }                
            }            
            infomessage("voici les lettre que tu a déja joué:");
                    Console.WriteLine(mauvaiselettre);
            return Lettretrouvé;
        }


        static char[] Lettre(){ //fonction qui vérifie si chaque lettre est identique a celle du mot cherché
            
            message("entrez une lettre",ref LettreJoueur);
            char essai = Convert.ToChar(LettreJoueur);
            bool mauvais=false;
            viebonus=false;
                for(int p=0;p<jeu.Length;p++){ //vérifie chaque lettre et affiche celles qui sont justes
                    if(jeu[p]==essai){
                        Lettretrouvé[p]=(essai);
                        viebonus=true;
                }
                else if(jeu[p]!=essai){
                    for(int m=0;m<mauvaiselettre.Length;m++){
                        if(mauvaiselettre[m]==Convert.ToChar("/") && mauvais==false){
                            mauvaiselettre[m]=essai;
                            mauvais=true;
                            break;   
                        }   
                    }   
                }   
            }
            if(viebonus==false){
            nbrVie--;
            combo=0;
            pendu=dessin(nbrVie);
            infomessage(pendu);
            } 
            infomessage("voici les lettre que tu a déja joué:");
                    Console.WriteLine(mauvaiselettre);

            return Lettretrouvé;
        }

        static void bonus(){//fonction qui détermine si on gagne une vie en cas de plusieurs lettre trouvé d'affilé

                
            if (viebonus==true){//vérifie si on a le droit a un bonus
                combo=combo+1;
                if(nbrVie<7 && combo>1){
                    //if (combo>1){//si on a assez de lettre de suite donne une vie 
                    nbrVie=nbrVie+1;
                    //Console.WriteLine(nbrVie-1);
                    infomessage("bravo vous avez trouvé "+combo+" lettre d'affilé voici un bonus d'une vie");            
                    pendu=dessin(nbrVie-1);//on soustrait 1 de nbrvie car les cases du tableau vont de 0 à 6 et non de 1 à 7
                    infomessage(pendu);
                    //}
                }
            }
        }

        //----------FONCTIONS DE RESTART ET DE DETECTION DE VICTOIRE-------

        static void restart(){//fonction qui demande au joueur si il veut rejouer ou non
            
            bool fin=false;
            while(fin==false){
            message("rejouer ? taper oui pour rejouer et non pour ne plus jouer",ref rejouer); 
            if(rejouer=="oui"){
                fin=true;
            }
            else if(rejouer=="non"){
                IsApplicationOn=false;
                infomessage("au revoir!");
                fin=true;
            }
            else{
                errormessage("taper oui ou non seulement");                
            }
            }
        }
            static bool victoire1(){//check la victoire si chaque lettre est la
                bool tiret=true;
                for(int h=0;h<Lettretrouvé.Length;h++){
                    if(Lettretrouvé[h]==Convert.ToChar("-")){
                        tiret=false;
                    }
                } 
                return tiret;
            }
            static bool victoire2(){//check la victoire si le mot tapé est le bon
                bool testvictoire=false;
                if(MotJoueur==motcache){
                    testvictoire=true;
                }
                else{
                    nbrVie--;
                    pendu=dessin(nbrVie);
                    infomessage(pendu);    
                }
                return testvictoire;
            }                
        }
    }

  