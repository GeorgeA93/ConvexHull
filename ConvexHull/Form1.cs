using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;


namespace ConvexHull
{
    public partial class convexHull : Form
    {
        #region Fields and properties

        //graphics for the gui
        Graphics graphics;
        //points selected by the user
        private List<Vector2> points = new List<Vector2>(); 
        //points that make up the hull
        private List<Vector2> hullPoints = new List<Vector2>();

        #endregion

        #region Constructor
        //constructor for the form
        public convexHull()
        {
            InitializeComponent();
        }

        //form load event
        private void convexHull_Load(object sender, EventArgs e)
        {
            graphics = this.CreateGraphics();
        }

        #endregion

        #region Convex Hull non DC
        //sort a list of points by its x coordinate
        private List<Vector2> SortListByX(List<Vector2> p)
        {
            var ordered = from point in p
                          orderby point.X, point.Y
                          select point;

            var orderedList = ordered.ToList();
            return orderedList;
        }

        //construct an Andrews Monotone Chain Convex Hull
        private List<Vector2> ConvexHull(List<Vector2> p)
        {
            p = SortListByX(p);

            List<Vector2> Upper = UpperHull(p);
            List<Vector2> Lower = LowerHull(p);

            return Upper.Union(Lower).ToList();
        }

        //construct the upper hull
        private List<Vector2> UpperHull(List<Vector2> p)
        {
            int n = p.Count();
            List<Vector2> Lupper = new List<Vector2>();
            Lupper.Add(p[0]);
            Lupper.Add(p[1]);

            for (int i = 2; i <= n - 1; i++)
            {
                Lupper.Add(p[i]);
                while (Lupper.Count > 2 && !isRightTurn(Lupper[Lupper.Count() - 3], Lupper[Lupper.Count() - 2], Lupper[Lupper.Count() - 1]))
                {
                    Lupper.Remove(Lupper[Lupper.Count() - 2]);
                }
            }
            return Lupper;
        }

        //construct the lower hull
        private List<Vector2> LowerHull(List<Vector2> p)
        {
            int n = p.Count() - 1;
            List<Vector2> Llower = new List<Vector2>();
            Llower.Add(p[n]);
            Llower.Add(p[n - 1]);

            for (int i = n - 2; i >= 1; i--)
            {
                Llower.Add(p[i]);
                while (Llower.Count > 2 && !isRightTurn(Llower[Llower.Count() - 3], Llower[Llower.Count() - 2], Llower[Llower.Count() - 1]))
                {
                    Llower.Remove(Llower[Llower.Count() - 2]);
                }
            }

            return Llower;
        }

        //calculate whether a point has turn right by using the cross product of three points
        private Boolean isRightTurn(Vector2 first, Vector2 middle, Vector2 last)
        {
            int crossProduct = (int)(((middle.X - first.X) * (last.Y - first.Y)) - ((middle.Y - first.Y) * (last.X - first.X)));
            if (crossProduct < 0)
            {
                return true;
            }
            else if (crossProduct > 0)
            {
                return false;
            }
            else return false;

        }

        //return an integer value of the cross product of three points
        private int isRightTurnInt(Vector2 first, Vector2 middle, Vector2 last)
        {
            int crossProduct = (int)(((middle.X - first.X) * (last.Y - first.Y)) - ((middle.Y - first.Y) * (last.X - first.X)));

            return crossProduct;

        }
        #endregion

        #region Convex Hull DC
        //construct a divide and conquer hull
        private List<Vector2> DCHull(List<Vector2> p)
        {
            //sort the list of points
            p = SortListByX(p);
            if (p.Count <= 3)
            {
                return ConvexHull(p);
            }
            else
            {
                //split the list into two seprate lists
                List<Vector2> firstHalf = new List<Vector2>();
                List<Vector2> secondHalf = new List<Vector2>();
                firstHalf = getHalf(p, firstHalf, 0, (p.Count() / 2));
                secondHalf = getHalf(p, secondHalf, p.Count() / 2, p.Count);
                //generate the convex huls of these two lists
                firstHalf = ConvexHull(firstHalf);
                secondHalf = ConvexHull(secondHalf);
                //merge the two hulls
                return MergeHulls(firstHalf, secondHalf);
            }            
        }

        //merged the two hulls
        private List<Vector2> MergeHulls(List<Vector2> Ha, List<Vector2> Hb)
        {
            List<Vector2> mergedHull = new List<Vector2>();
            List<Vector2> lowerTangent = new List<Vector2>();
            List<Vector2> upperTangent = new List<Vector2>();

            Ha.Reverse(0, Ha.Count);
            Hb.Reverse(0, Hb.Count);

            lowerTangent = getTangent(Hb, Ha);
            upperTangent = getTangent(Ha, Hb);

            mergedHull = Ha.Union(Hb).ToList();
            mergedHull.AddRange(lowerTangent);
            mergedHull.AddRange(upperTangent);
     
            for (int i = 0; i < mergedHull.Count; i++)
            {
                if(InHull(mergedHull, mergedHull[i])){
                    mergedHull.RemoveAt(i);
                }
            }

            return mergedHull;
        }

        //get the tangent between two hulls
        //the lower is calculated by calling it with Hb, Ha
        //the upper i calulated by calling it with Ha, Hb
        private List<Vector2> getTangent(List<Vector2> Ha, List<Vector2> Hb)
        {
            List<Vector2> tangent = new List<Vector2>();

            int indexHa, indexHb;
            indexHa = getMaxXIndex(Ha);
            indexHb = getMinXIndex(Hb);

            Boolean done = false;
            while (!done)
            {
                done = true;
                while (isRightTurnInt(Hb[indexHb], Ha[indexHa], Ha[indexHa + 1]) >= 0)
                {
                    ++indexHa;
                }
                while (isRightTurnInt(Ha[indexHa], Hb[indexHb], Hb[indexHb - 1]) <= 0)
                {
                    --indexHb;
                    done = false;
                }
            }
            tangent.Add(Ha[indexHa]);
            tangent.Add(Hb[indexHb]);
            return tangent;
        }

       
        //check whether a certain point is in the hull.
        public static Boolean InHull(List<Vector2> hull, Vector2 point)
        {
            Vector2 pointA, pointB;
            Boolean insideHull = false;
            if (hull.Count < 3)
            {
                return insideHull;
            }
            Vector2 oldPoint = hull[hull.Count - 1];

            for (int i = 0; i < hull.Count; i++)
            {
                Vector2 newPoint = hull[i];

                if (newPoint.X > oldPoint.X)
                {
                    pointA = oldPoint;
                    pointB = newPoint;
                }
                else
                {
                    pointA = newPoint;
                    pointB = oldPoint;
                }
                if ((newPoint.X < point.X) == (point.X <= oldPoint.X)
                    && (point.Y - (long)pointA.Y) * (pointB.X - pointA.X)
                    < (pointB.Y - (long)pointA.Y) * (point.X - pointA.X))
                {
                    insideHull = !insideHull;
                }
                oldPoint = newPoint;
            }
            return insideHull;
        }

        //get the right most x index in the hull
        private int getMaxXIndex(List<Vector2> Hull)
        {
            Vector2 maxX = Hull[0];
            int index = 0;
            for (int i = 0; i < Hull.Count(); i++)
            {
                if (Hull[i].X > maxX.X)
                {
                    maxX = Hull[i];
                    index = i;
                }
            }
            return index;
        }

        //get the left most x index in the hull
        private int getMinXIndex(List<Vector2> Hull)
        {
            Vector2 minX = Hull[0];
            int index = 0;
            for (int i = 0; i < Hull.Count(); i++)
            {
                if (Hull[i].X < minX.X)
                {
                    minX = Hull[i];
                    index = i;
                }
            }
            return index;
        }

        //split the set of points in half
        private List<Vector2> getHalf(List<Vector2> oldList, List<Vector2> newList, int start, int finish)
        {
            for (int i = start; i < finish; i++)
            {
                newList.Add(oldList[i]);
            }
            return newList;
        }


        #endregion

        #region Drawing

        //drawing points
        private void renderPoint(int x, int y, System.Drawing.Color c)
        {
            Pen p = new Pen(c, 2f);
            graphics.DrawEllipse(p, x - 5, y - 5, 5, 5);
        }

        //drawing lines
        private void renderLine(Vector2 v1, Vector2 v2, System.Drawing.Color c)
        {
            Pen p = new Pen(c, 2f);
            PointF p1 = new PointF(v1.X, v1.Y);
            PointF p2 = new PointF(v2.X, v2.Y);
            graphics.DrawLine(p, p1, p2);
        }
        #endregion


        #region Events
        //mouse down event
        private void convexHull_MouseDown(object sender, MouseEventArgs e)
        {
            points.Add(new Vector2(e.X, e.Y));
            renderPoint(e.X, e.Y, System.Drawing.Color.Black);
        }


        //Compute button event
        private void compute_Click(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                hullPoints = ConvexHull(points);
                foreach (Vector2 p in hullPoints)
                {
                    renderPoint((int)p.X, (int)p.Y, System.Drawing.Color.Red);
                }
                for (int i = 0; i < hullPoints.Count(); i++)
                {
                    if (i + 1 != hullPoints.Count())
                    {
                        renderLine(hullPoints[i], hullPoints[i + 1], System.Drawing.Color.Red);
                    }
                    else
                    {
                        renderLine(hullPoints[i], hullPoints[0], System.Drawing.Color.Red);
                    }
                }
            }
            else
            {
                //generate a random number of points
                Random rnd = new Random();
                for(int i = 0; i < 100; i++)
                {
                    int x = rnd.Next(20, 500);
                    int y = rnd.Next(20, 300);
                    points.Add(new Vector2(x, y));
                    renderPoint(x, y, System.Drawing.Color.Black);
                }     
                hullPoints = ConvexHull(points);
                foreach (Vector2 p in hullPoints)
                {
                    renderPoint((int)p.X, (int)p.Y, System.Drawing.Color.Red);
                }
               

            }
        }

        //D and C Compute button event
        private void dcCompute_Click(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                hullPoints = DCHull(points);

                foreach (Vector2 p in hullPoints)
                {
                    renderPoint((int)p.X, (int)p.Y, System.Drawing.Color.Red);
                }                                        
            }
            else
            {
                //generate a random number of points
                Random rnd = new Random();
                for (int i = 0; i < 10; i++)
                {
                    int x = rnd.Next(20, 500);
                    int y = rnd.Next(20, 300);
                    points.Add(new Vector2(x, y));
                    renderPoint(x, y, System.Drawing.Color.Black);
                }
                hullPoints = DCHull(points);
                foreach (Vector2 p in hullPoints)
                {
                    renderPoint((int)p.X, (int)p.Y, System.Drawing.Color.Red);
                }               
            }
        }

        //clear the screen
        private void clear_Click(object sender, EventArgs e)
        {
            graphics.Clear(System.Drawing.Color.Silver);
            points.Clear();
            hullPoints.Clear();
        }
        #endregion
    }
}
