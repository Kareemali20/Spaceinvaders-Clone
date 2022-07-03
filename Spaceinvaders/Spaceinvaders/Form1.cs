using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Spaceinvaders
{

    // Classes
    class Bullet
    {
        public int X, Y;
        public Bitmap Image;
        public int BulletSpeed;
        public int BulletType;

        public Bullet()
        {
            X = Y = 0;
            Image = null;
            BulletSpeed = 15;
            BulletType = 0;
        }
    }

    class AssetWithFrames
    {
        public int X, Y;
        public List<Bitmap> Images = new List<Bitmap>();
        public int Width, Height;
        public int WhichFrame;
        public int Speed;
        public int WhichDirection;
        public int BulletHitCount;

        public AssetWithFrames()
        {
            X = Y = Width = Height = WhichFrame = 0;
            Speed = 0;
            WhichDirection = 0; // 0 -> W , 1 -> S , 2 -> A , 3 -> D
            BulletHitCount = 0;
        }
    }

    class AdvancedImageAssetWithFrames
    {
        public int X, Y;
        public List<Bitmap> Images = new List<Bitmap>();
        public int WhichFrame = 0;
        public List<int> Width = new List<int>();
        public List<int> Height = new List<int>();


        public AdvancedImageAssetWithFrames()
        {
            X = Y = 0;
        }
    }

    class AdvancedImageAssetWithoutFrames
    {
        public int X, Y;
        public Bitmap Image;
        public int Width, Height;
        public int BulletCount;
        public AdvancedImageAssetWithoutFrames()
        {
            X = Y = Width = Height = 0;
            Image = null;
            BulletCount = 0;
        }
    }

    class Shape
    {
        public int X, Y;
        public int Height, Width;
        public Color Clr;

        public Shape()
        {
            X = Y = Height = Width = 0;
            Clr = Color.White;
        }
    }



    public partial class Form1 : Form
    {

        // Variables 
        Bitmap Off;
        Timer T = new Timer();
        int TickCounter = 0;
        bool Explode = false;
        int Speed = 10;
        int Count = 0;
        bool KeyDownOrUp = false;
        int XS = 0;
        bool LeftOrRight = false;
        bool NewLevel = false;
        bool Create = false;
        int Counter = 15;
        bool Scroll1 = false;
        bool Jump = false;
        int Spacing2 = 0;
        bool LadderClimb = false;

        // Lists
        List<AdvancedImageAssetWithFrames> Backgrounds = new List<AdvancedImageAssetWithFrames>();
        List<AdvancedImageAssetWithFrames> Backgrounds2 = new List<AdvancedImageAssetWithFrames>();
        List<AdvancedImageAssetWithoutFrames> Background3 = new List<AdvancedImageAssetWithoutFrames>();

        List<AdvancedImageAssetWithFrames> Rockets = new List<AdvancedImageAssetWithFrames>();
        List<AdvancedImageAssetWithFrames> Explosion = new List<AdvancedImageAssetWithFrames>();


        List<AdvancedImageAssetWithoutFrames> Enemies = new List<AdvancedImageAssetWithoutFrames>();
        List<AdvancedImageAssetWithoutFrames> GameOver = new List<AdvancedImageAssetWithoutFrames>();

        List<AssetWithFrames> Hero = new List<AssetWithFrames>();
        List<AssetWithFrames> Health = new List<AssetWithFrames>();

        List<AdvancedImageAssetWithoutFrames> Explosion2 = new List<AdvancedImageAssetWithoutFrames>();

        List<AdvancedImageAssetWithoutFrames> Ladder = new List<AdvancedImageAssetWithoutFrames>();
        List<AdvancedImageAssetWithFrames> LadderFrames = new List<AdvancedImageAssetWithFrames>();


        List<AdvancedImageAssetWithFrames> Astronaut = new List<AdvancedImageAssetWithFrames>();
        List<Bullet> Bullets = new List<Bullet>();
        List<Bullet> EnemyBullets = new List<Bullet>();


        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            Load += Form1_Load1;
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            MouseDown += Form1_MouseDown;

            // Timer Event
            T.Tick += T_Tick;
            T.Start();

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!NewLevel)
            {
                if (e.Button == MouseButtons.Left)
                {
                    CreateBullet(true);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    CreateMultipleBullet();
                }
            }

        }

        // Events
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!NewLevel)
            {

                Hero[0].WhichFrame = 0;
                KeyDownOrUp = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!NewLevel)
            {
                KeyDownOrUp = false;
                switch (e.KeyCode)
                {
                    case Keys.W:
                        Hero[0].WhichFrame = 0;
                        Hero[0].Y -= Hero[0].Speed;
                        Hero[0].WhichDirection = 0;
                        Hero[0].Speed++;
                        break;
                    case Keys.A:
                        Hero[0].WhichFrame = 1;
                        Hero[0].X -= Hero[0].Speed;
                        Hero[0].WhichDirection = 2;
                        Hero[0].Speed++;
                        break;
                    case Keys.S:
                        Hero[0].WhichFrame = 0;
                        Hero[0].Y += Hero[0].Speed;
                        Hero[0].WhichDirection = 1;
                        Hero[0].Speed++;
                        break;
                    case Keys.D:
                        Hero[0].WhichFrame = 2;
                        Hero[0].X += Hero[0].Speed;
                        Hero[0].WhichDirection = 3;
                        Hero[0].Speed++;
                        break;

                }
            }
            else
            {
                if (Astronaut.Count != 0)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.W:
                            Astronaut[0].Y -= 5;
                            if (Astronaut[0].X > Ladder[0].X && Astronaut[0].X < Ladder[0].X + 100)
                            {
                                LadderClimb = true;
                                if (LadderFrames.Count != 1)
                                {
                                    CreateLadderFrames();
                                    Astronaut.Clear();

                                }
                            }
                            else
                            {
                                LadderClimb = false;
                                LadderFrames.Clear();
                            }

                            break;
                        case Keys.A:
                            Astronaut[0].X -= 5;
                            break;
                        case Keys.S:
                            Astronaut[0].Y += 5;
                            break;
                        case Keys.D:
                            Astronaut[0].X += 5;
                            break;
                        case Keys.Space:
                            Jump = true;
                            break;

                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                            LadderFrames[0].Y -= 5;
                            LadderFrames[0].WhichFrame++;
                            break;
                        case Keys.Down:
                            LadderFrames[0].Y += 5;
                            LadderFrames[0].WhichFrame++;

                            break;
                        case Keys.Right:
                            LadderFrames[0].X += 5;

                            break;
                        case Keys.Left:
                            LadderFrames[0].X -= 5;
                            break;
                    }
                }

            }

        }


        // Timer Event
        private void T_Tick(object sender, EventArgs e)
        {
            // Changing Backgrounds

            if (!NewLevel)
            {
                if (TickCounter % 50 == 0 && TickCounter != 0)
                {
                    for (int i = 0; i < Backgrounds.Count; i++)
                    {
                        Backgrounds[i].WhichFrame++;
                        if (Backgrounds[i].WhichFrame == 2)
                        {
                            Backgrounds[i].WhichFrame = 0;
                        }
                    }
                }

                // Animating the rockets
                AnimateRockets();
                if (TickCounter % 5 == 0)
                {
                    for (int i = 0; i < Rockets.Count; i++)
                    {
                        Rockets[i].WhichFrame = 1;
                    }
                }
                if (Explode)
                {
                    // Explosion Animation
                    for (int i = 0; i < Explosion.Count; i++)
                    {
                        Explosion[i].WhichFrame++;
                        if (Explosion[i].WhichFrame == 10)
                        {
                            Explosion.Clear();
                            Explode = false;
                            CreateRocket();
                            Speed += 3;
                            if (Speed > 40)
                            {
                                Speed = 15;
                            }

                        }
                    }
                }

                // Animating the bullets
                AnimateBullets();

                // Animating the enemies 
                AnimateEnemies();

                // Hero Movement
                if (KeyDownOrUp)
                {
                    for (int i = 0; i < Hero.Count; i++)
                    {
                        if (Hero[i].Speed != 0)
                        {
                            if (Hero[i].WhichDirection == 0)
                            {
                                Hero[i].Y -= Hero[i].Speed;
                            }
                            else if (Hero[i].WhichDirection == 1)
                            {
                                Hero[i].Y += Hero[i].Speed;

                            }
                            else if (Hero[i].WhichDirection == 2)
                            {
                                Hero[i].X -= Hero[i].Speed;

                            }
                            else if (Hero[i].WhichDirection == 3)
                            {
                                Hero[i].X += Hero[i].Speed;

                            }

                            Hero[i].Speed--;
                        }


                    }

                }

                for (int i = 0; i < Bullets.Count; i++)
                {
                    if (Bullets[i].Y < 0)
                    {
                        Count++;
                    }
                }
                if (Count == Bullets.Count)
                {
                    Bullets.Clear();
                    Explosion2.Clear();

                    Count = 0;
                }
                else
                {
                    Count = 0;
                }

                // Checking Collision
                for (int i = 0; i < Bullets.Count; i++)
                {
                    CheckBulletCollision(Bullets[i].X, Bullets[i].Y, i);
                    for (int k = 0; k < Enemies.Count; k++)
                    {
                        if (Enemies[k].BulletCount == 3)
                        {
                            Enemies.Remove(Enemies[k]);
                        }
                    }
                }


                for (int i = 0; i < EnemyBullets.Count; i++)
                {
                    CheckEnemyBulletCollision(EnemyBullets[i].X, EnemyBullets[i].Y, i);
                }

                // Checking Rocket Collision
                for (int i = 0; i < Rockets.Count; i++)
                {
                    CheckRocketCollision(Rockets[i].X, Rockets[i].Y);
                }

                // Creating Enemy Bullets
                if (TickCounter % 10 == 0)
                {
                    if (Hero.Count != 0)
                    {
                        CreateEnemyBullet();
                    }
                }

                // Switching to next level
                if (Enemies.Count == 0 && !NewLevel)
                {
                    Clear();
                    NewLevel = true;
                }

                TickCounter++;
            }
            else
            {
                if (!Create)
                {
                    CreateBackgrounds2();
                    CreateBackgrounds3();
                    Create = true;
                    CreateAstornaut();
                    CreateLadder();
                }


                if (Jump)
                {
                    if (Astronaut[0].WhichFrame != 3)
                    {
                        Astronaut[0].WhichFrame++;

                    }
                    else if (Astronaut[0].WhichFrame == 3)
                    {
                        while (Astronaut[0].WhichFrame != 0)
                        {
                            Astronaut[0].WhichFrame--;

                        }
                        Jump = false;

                    }


                }
                if (LadderClimb)
                {
                    if (LadderFrames[0].WhichFrame == 3)
                    {
                        LadderFrames[0].WhichFrame = 0;
                    }

                }


                TickCounter++;
            }

            DoubleBuffer(this.CreateGraphics());
        }

        // Paint Event
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DoubleBuffer(e.Graphics);
        }


        // Load Event
        private void Form1_Load1(object sender, EventArgs e)
        {
            Off = new Bitmap(this.Width, this.Height);

            CallFunctions();
        }



        // Functions
        void CallFunctions()
        {
            CreateBackgrounds();
            CreateHero();
            CreateHealthBar();
            CreateEnemies();
            CreateRocket();
        }

        void ClearAndGameOver()
        {
            Clear();
            CreateGameOver();
        }
        void Clear()
        {
            Hero.Clear();
            Enemies.Clear();
            Rockets.Clear();
            Bullets.Clear();
            EnemyBullets.Clear();
            Health.Clear();
            Explosion.Clear();
            Explosion2.Clear();
            Backgrounds.Clear();
            EnemyBullets.Clear();
        }



        // Assets Creation Functions
        void CreateBackgrounds()
        {
            AdvancedImageAssetWithFrames B = new AdvancedImageAssetWithFrames();
            Bitmap I;

            B.X = 0;
            B.Y = 0;

            for (int i = 0; i < 3; i++)
            {
                I = new Bitmap("Background" + (i + 1) + ".jpg");
                B.Width.Add(I.Width);
                B.Height.Add(I.Height);
                B.Images.Add(I);
            }

            Backgrounds.Add(B);

        }

        void CreateBackgrounds2()
        {
            AdvancedImageAssetWithFrames B = new AdvancedImageAssetWithFrames();
            Bitmap I;

            B.X = 0;
            B.Y = 0;

            for (int i = 1; i < 3; i++)
            {
                I = new Bitmap("Background" + (i + 1) + ".jpg");
                B.Width.Add(I.Width);
                B.Height.Add(I.Height);
                B.Images.Add(I);
            }

            Backgrounds2.Add(B);

        }
        void CreateBackgrounds3()
        {
            AdvancedImageAssetWithoutFrames B = new AdvancedImageAssetWithoutFrames();
            B.X = 0;
            B.Y = 0;
            B.Image = new Bitmap("Background3.jpg");
            Background3.Add(B);
        }


        void CreateHero()
        {
            Bitmap Image;
            AssetWithFrames A = new AssetWithFrames();
            A.X = this.Width / 2 - 120;
            A.Y = this.Height - 200;


            for (int i = 0; i < 3; i++)
            {
                Image = new Bitmap("Hero" + (i + 1) + ".png");

                A.Images.Add(Image);
            }
            A.Width = A.Images[0].Width;
            A.Height = A.Images[0].Height;

            Hero.Add(A);
        }

        void CreateHealthBar()
        {
            Bitmap Image;
            AssetWithFrames A = new AssetWithFrames();
            A.X = 10;

            for (int i = 0; i < 5; i++)
            {
                Image = new Bitmap("Health" + (i + 1) + ".png");

                A.Images.Add(Image);
            }

            A.Width = A.Images[0].Width;
            A.Height = A.Images[0].Height;
            A.Y = this.ClientSize.Height - A.Height - 30;

            Health.Add(A);
        }

        void CreateEnemies()
        {
            int XSpacing = 0;
            int YSpacing = 0;

            int H = 0;
            AdvancedImageAssetWithoutFrames E;
            Bitmap I;

            // Creating 3 types of enemy
            for (int i = 0; i < 3; i++)
            {
                XSpacing = 0;
                for (int k = 0; k < 10; k++)
                {
                    E = new AdvancedImageAssetWithoutFrames();
                    E.X = 300 + XSpacing;
                    E.Y = 70 + YSpacing;
                    I = new Bitmap("Enemy" + (i + 1) + ".png");
                    E.Image = I;
                    E.Width = I.Width;
                    E.Height = I.Height;
                    H = I.Height;

                    XSpacing += E.Width - 70;
                    Enemies.Add(E);
                }
                YSpacing += H - 50;
            }
        }

        void CreateRocket()
        {
            AdvancedImageAssetWithFrames E;
            Random R = new Random();
            Bitmap I;


            for (int i = 0; i < 2; i++)
            {
                if (XS == 0)
                {
                    XS = Enemies[29].X + 100;
                }
                E = new AdvancedImageAssetWithFrames();
                for (int k = 0; k < 2; k++)
                {
                    I = new Bitmap("Rocket" + (k + 1) + ".png");
                    E.Images.Add(I);
                    E.Height.Add(I.Height);
                    E.Width.Add(I.Width);
                }

                if (i == 0)
                {
                    E.X = R.Next(100, 200);
                }
                else
                {
                    E.X = R.Next(XS, this.Width - 100);
                }
                E.Y = 100;

                Rockets.Add(E);
            }
        }

        void CreateBullet(bool EnemyOrHero)
        {
            Bullet B = new Bullet();
            if (EnemyOrHero)
            {
                B.X = Hero[0].X + (Hero[0].Width / 4) - 2;
                B.Y = Hero[0].Y - 30;
                B.Image = new Bitmap("Bullet1.png");

                Bullets.Add(B);
            }


        }

        void CreateExplosions()
        {
            AdvancedImageAssetWithFrames E;
            Bitmap I = null;

            for (int k = 0; k < 2; k++)
            {
                E = new AdvancedImageAssetWithFrames();
                for (int i = 0; i < 10; i++)
                {
                    I = new Bitmap("Explosion" + (i + 1) + ".png");
                    E.Images.Add(I);
                }
                E.X = Rockets[k].X - 100;
                E.Y = this.ClientSize.Height - 270;
                Explosion.Add(E);
            }
            Rockets.Clear();

        }

        void CreateMultipleBullet()
        {

            Bullet B;
            int Spacing = 0;
            for (int i = 0; i < 3; i++)
            {
                B = new Bullet();
                B.X = Hero[0].X + (Hero[0].Width / 7) + Spacing;
                B.Y = Hero[0].Y - 30;
                B.Image = new Bitmap("Bullet3.png");
                B.BulletType = 1;
                Bullets.Add(B);
                Spacing += 20;
            }
        }

        void CreateEnemyBullet()
        {
            Random R = new Random();
            int r;
            if (Enemies.Count != 0)
            {
                r = R.Next(0, Enemies.Count - 1);
                Bullet B = new Bullet();
                B.X = Enemies[r].X + Enemies[r].Width / 5;
                B.Y = Enemies[r].Y + 60;
                B.Image = new Bitmap("Bullet2.png");
                EnemyBullets.Add(B);
            }


        }

        void CreateGameOver()
        {
            AdvancedImageAssetWithoutFrames G = new AdvancedImageAssetWithoutFrames();
            G.X = this.Width - this.Width / 2 - 200;
            G.Y = this.Height - this.Height / 2 - 200;
            G.Image = new Bitmap("Gameover.png");
            GameOver.Add(G);

        }

        void CreateAstornaut()
        {
            AdvancedImageAssetWithFrames A = new AdvancedImageAssetWithFrames();
            A.X = 300;
            A.Y = this.Height - 250;
            for (int i = 0; i < 4; i++)
            {
                A.Images.Add(new Bitmap("Astronaut" + (i + 1) + ".png"));
                A.Width.Add(A.Images[i].Width);
                A.Height.Add(A.Images[i].Height);
            }
            Astronaut.Add(A);
        }

        void CreateLadder()
        {
            AdvancedImageAssetWithoutFrames L;
            for (int i = 0; i < 4; i++)
            {
                L = new AdvancedImageAssetWithoutFrames();
                L.X = 700;
                L.Y = 600 - Spacing2;
                L.Image = new Bitmap("Ladder.png");
                L.Width = L.Image.Width;
                L.Height = L.Image.Height;

                Ladder.Add(L);
                Spacing2 += 85;
            }
        }

        void CreateLadderFrames()
        {
            AdvancedImageAssetWithFrames A = new AdvancedImageAssetWithFrames();
            A.X = Astronaut[0].X + 25;
            A.Y = Astronaut[0].Y;
            for (int i = 0; i < 4; i++)
            {
                A.Images.Add(new Bitmap("Ladder" + (i + 1) + ".png"));
                A.Width.Add(A.Images[i].Width);
                A.Height.Add(A.Images[i].Height);
            }
            LadderFrames.Add(A);
        }


        // Animation Functions
        void AnimateBullets()
        {
            int LeftOrRight = 0;
            Random R = new Random();
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].Y -= Bullets[i].BulletSpeed;
                if (Bullets[i].Y < 0)
                {

                    Bullets.RemoveAt(i);
                }
                else
                {
                    if (Bullets[i].BulletType == 1)
                    {
                        LeftOrRight = R.Next(0, 2);
                        if (LeftOrRight == 0)
                        {
                            Bullets[i].X -= 10;

                        }
                        else
                        {
                            Bullets[i].X += 10;

                        }
                    }
                    Bullets[i].BulletSpeed++;
                }



            }

            for (int i = 0; i < EnemyBullets.Count; i++)
            {
                EnemyBullets[i].Y += EnemyBullets[i].BulletSpeed;
                if (EnemyBullets[i].Y > this.Height)
                {

                    EnemyBullets.RemoveAt(i);
                }
            }
        }

        void AnimateEnemies()
        {


            for (int i = 0; i < Enemies.Count; i++)
            {
                if (!LeftOrRight)
                {
                    Enemies[i].X -= 10;
                    if (Enemies[0].X < 200)
                    {
                        LeftOrRight = true;
                        break;
                    }

                }
                else
                {
                    Enemies[i].X += 10;

                    if (Enemies[i].X > this.Width - 200)
                    {
                        LeftOrRight = false;

                    }
                }
            }
        }

        void AnimateRockets()
        {
            for (int i = 0; i < Rockets.Count; i++)
            {
                if (Rockets[i].Y + 100 < this.ClientSize.Height)
                {
                    Rockets[i].Y += Speed;

                }
                else
                {
                    CreateExplosions();
                    Explode = true;

                }
            }
        }



        // Collision Functions

        void CheckBulletCollision(int X, int Y, int BulletNumber)
        {
            int XStart, XEnd, YStart, YEnd;
            for (int i = 0; i < Enemies.Count; i++)
            {
                XStart = Enemies[i].X - 30;
                XEnd = Enemies[i].X + 50;
                YStart = Enemies[i].Y;
                YEnd = Enemies[i].Y + 100;

                if (X >= XStart && X <= XEnd && Y >= YStart && Y <= YEnd)
                {
                    Bullets.RemoveAt(BulletNumber);
                    Enemies[i].BulletCount++;
                    AdvancedImageAssetWithoutFrames E = new AdvancedImageAssetWithoutFrames();
                    E.X = Enemies[i].X - (Enemies[i].Width / 9);
                    E.Y = Enemies[i].Y - 40;
                    E.Image = new Bitmap("Explosion5.png");
                    Explosion2.Add(E);
                }
            }
        }

        void CheckEnemyBulletCollision(int X, int Y, int BulletNumber)
        {
            int XStart, XEnd, YStart, YEnd;
            for (int i = 0; i < Hero.Count; i++)
            {
                XStart = Hero[i].X;
                XEnd = Hero[i].X + Hero[i].Width - 50;
                YStart = Hero[i].Y;
                YEnd = Hero[i].Y + Hero[i].Height;
                if (X >= XStart && X <= XEnd && Y >= YStart && Y <= YEnd)
                {

                    EnemyBullets.RemoveAt(BulletNumber);
                    Hero[i].BulletHitCount++;
                    if (Hero[i].BulletHitCount == 3)
                    {
                        Health[0].WhichFrame++;
                        if (Health[0].WhichFrame == 5)
                        {
                            Health[0].WhichFrame = 4;
                            ClearAndGameOver();
                            break;
                        }
                        Hero[i].BulletHitCount = 0;
                    }
                }
            }
        }

        void CheckRocketCollision(int X, int Y)
        {
            int XStart, XEnd, YStart, YEnd;
            for (int i = 0; i < Hero.Count; i++)
            {
                XStart = Hero[i].X;
                XEnd = Hero[i].X + Hero[i].Width;
                YStart = Hero[i].Y;
                YEnd = Hero[i].Y + Hero[i].Height;

                if (X >= XStart && X <= XEnd && Y >= YStart && Y <= YEnd)
                {
                    if (Health[0].WhichFrame < 4)
                    {
                        Health[0].WhichFrame++;


                    }
                    else if (Health[0].WhichFrame == 4)
                    {
                        ClearAndGameOver();

                    }


                }
            }
        }

        // Double Buffer
        void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);

            // Drawing the Background
            for (int i = 0; i < Backgrounds.Count; i++)
            {

                Rectangle Destination = new Rectangle(0, 0, this.Width, this.Height);
                Rectangle Source = new Rectangle(Backgrounds[i].X, Backgrounds[i].Y, Backgrounds[i].Width[Backgrounds[i].WhichFrame], Backgrounds[i].Height[Backgrounds[i].WhichFrame]);

                g.DrawImage(Backgrounds[i].Images[Backgrounds[i].WhichFrame], Destination, Source, GraphicsUnit.Pixel);

            }

            //Drawing the Hero
            for (int i = 0; i < Hero.Count; i++)
            {
                Rectangle Destination = new Rectangle(Hero[i].X, Hero[i].Y, 120, 120);
                Rectangle Source = new Rectangle(0, 0, Hero[i].Width, Hero[i].Height);

                g.DrawImage(Hero[i].Images[Hero[i].WhichFrame], Destination, Source, GraphicsUnit.Pixel);
            }

            // Drawing the Health bar
            for (int i = 0; i < Health.Count; i++)
            {
                Rectangle Destination = new Rectangle(Health[i].X, Health[i].Y, 120, 80);
                Rectangle Source = new Rectangle(0, 0, Health[i].Width, Health[i].Height);

                g.DrawImage(Health[i].Images[Health[i].WhichFrame], Destination, Source, GraphicsUnit.Pixel);
            }

            // Drawing the Enemies
            for (int i = 0; i < Enemies.Count; i++)
            {
                Rectangle Destination = new Rectangle(Enemies[i].X, Enemies[i].Y, 70, 70);
                Rectangle Source = new Rectangle(0, 0, Enemies[i].Width, Enemies[i].Height);

                g.DrawImage(Enemies[i].Image, Destination, Source, GraphicsUnit.Pixel);
            }

            // Drawing the Rockets
            for (int i = 0; i < Rockets.Count; i++)
            {

                Rectangle Destination = new Rectangle(Rockets[i].X, Rockets[i].Y, 100, 100);
                Rectangle Source = new Rectangle(0, 0, Rockets[i].Images[Rockets[i].WhichFrame].Width, Rockets[i].Images[Rockets[i].WhichFrame].Height);

                g.DrawImage(Rockets[i].Images[Rockets[i].WhichFrame], Destination, Source, GraphicsUnit.Pixel);
            }

            // Drawing the Bullet
            for (int i = 0; i < Bullets.Count; i++)
            {

                Rectangle Destination = new Rectangle(Bullets[i].X, Bullets[i].Y, 45, 45);
                Rectangle Source = new Rectangle(0, 0, Bullets[i].Image.Width, Bullets[i].Image.Height);

                g.DrawImage(Bullets[i].Image, Destination, Source, GraphicsUnit.Pixel);
            }

            // Drawing the enemy Bullet 
            for (int i = 0; i < EnemyBullets.Count; i++)
            {

                Rectangle Destination = new Rectangle(EnemyBullets[i].X, EnemyBullets[i].Y, 40, 40);
                Rectangle Source = new Rectangle(0, 0, EnemyBullets[i].Image.Width, EnemyBullets[i].Image.Height);

                g.DrawImage(EnemyBullets[i].Image, Destination, Source, GraphicsUnit.Pixel);
            }



            // Drawing the Explosion if needed
            if (Explode)
            {
                for (int i = 0; i < Explosion.Count; i++)
                {
                    Rectangle Destination = new Rectangle(Explosion[i].X, Explosion[i].Y, 300, 300);
                    Rectangle Source = new Rectangle(0, 0, Explosion[i].Images[Explosion[i].WhichFrame].Width, Explosion[i].Images[Explosion[i].WhichFrame].Height);

                    g.DrawImage(Explosion[i].Images[Explosion[i].WhichFrame], Destination, Source, GraphicsUnit.Pixel);

                }
            }

            for (int i = 0; i < Explosion2.Count; i++)
            {
                Rectangle Destination = new Rectangle(Explosion2[i].X, Explosion2[i].Y, 100, 100);
                Rectangle Source = new Rectangle(0, 0, Explosion2[i].Image.Width, Explosion2[i].Image.Height);

                g.DrawImage(Explosion2[i].Image, Destination, Source, GraphicsUnit.Pixel);
            }

            // Game Over
            for (int i = 0; i < GameOver.Count; i++)
            {

                Rectangle Destination = new Rectangle(GameOver[i].X, GameOver[i].Y, 300, 300);
                Rectangle Source = new Rectangle(0, 0, GameOver[i].Image.Width, GameOver[i].Image.Height);

                g.DrawImage(GameOver[i].Image, Destination, Source, GraphicsUnit.Pixel);
            }




            // Scrolling
            Counter = 5;
            if (Enemies.Count == 0)
            {
                for (int i = 0; i < Backgrounds2.Count; i++)
                {
                    if (Backgrounds2[0].X > Backgrounds2[0].Images[0].Width)
                    {
                        Scroll1 = true;
                    }
                    else
                    {

                        Rectangle Destination = new Rectangle(0, 0, this.Width - Counter, this.Height);
                        Rectangle Source = new Rectangle(Backgrounds2[i].X += Counter, Backgrounds2[i].Y, Backgrounds2[i].Width[i + 1], Backgrounds2[i].Height[i + 1]);

                        g.DrawImage(Backgrounds2[i].Images[i], Destination, Source, GraphicsUnit.Pixel);

                    }
                    Counter++;

                }
            }


            if (Scroll1)
            {
                for (int i = 0; i < Background3.Count; i++)
                {
                    Rectangle Destination = new Rectangle(0, 0, this.Width, this.Height);
                    Rectangle Source = new Rectangle(Background3[i].X, Background3[i].Y, Background3[i].Image.Width, Background3[i].Image.Height);

                    g.DrawImage(Background3[i].Image, Destination, Source, GraphicsUnit.Pixel);
                }

            }

            // Drawing the Ladder
            for (int i = 0; i < Ladder.Count; i++)
            {
                Rectangle Destination = new Rectangle(Ladder[i].X, Ladder[i].Y - 200, 200, 200);
                Rectangle Source = new Rectangle(0, 0, Ladder[i].Width, Ladder[i].Height);

                g.DrawImage(Ladder[i].Image, Destination, Source, GraphicsUnit.Pixel);

            }

            // Drawing the astronaut
            for (int i = 0; i < Astronaut.Count; i++)
            {

                Rectangle Destination = new Rectangle(Astronaut[i].X, Astronaut[i].Y, 150, 150);
                Rectangle Source = new Rectangle(0, 0, Astronaut[i].Images[Astronaut[i].WhichFrame].Width, Astronaut[i].Images[Astronaut[i].WhichFrame].Height);

                g.DrawImage(Astronaut[i].Images[Astronaut[i].WhichFrame], Destination, Source, GraphicsUnit.Pixel);
            }

            // Drawing the ladderFrames
            for (int i = 0; i < LadderFrames.Count; i++)
            {
                Rectangle Destination = new Rectangle(LadderFrames[i].X, LadderFrames[i].Y, 100, 100);
                Rectangle Source = new Rectangle(0, 0, LadderFrames[i].Width[i], LadderFrames[i].Height[i]);

                g.DrawImage(LadderFrames[i].Images[LadderFrames[i].WhichFrame], Destination, Source, GraphicsUnit.Pixel);
            }

        }

        void DoubleBuffer(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(Off);
            DrawScene(g2);
            g.DrawImage(Off, 0, 0);
        }
    }
}
