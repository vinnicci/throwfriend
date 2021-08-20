using Godot;

[Tool]
class PolyMeshTool : Polygon2D
{
    bool autoPoly;
    [Export] bool AutoPoly {
        get {
            return autoPoly;
        }
        set {
            autoPoly = value;
            currentPolyCount = Polygon.Length;
            if(autoPoly) {
                SetProcess(true);
            }
            else {
                SetProcess(false);
            }
        }
    }


    public override void _Ready()
    {
        base._Ready();
        AutoPoly = AutoPoly;
    }


    int currentPolyCount;


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(AutoPoly && currentPolyCount != Polygon.Length) {
            CreatePoly();
            currentPolyCount = Polygon.Length;
        }
    }


    void CreatePoly() {
        if(Polygon.Length < 3) {
            return;
        }
        Polygons = new Godot.Collections.Array();
		int[] delaunayPts = Godot.Geometry.TriangulateDelaunay2d(Polygon);
		for(int i = 0; i <= (delaunayPts.Length / 3) - 1; i++) {
			int[] poly = new int[3];
			for(int k = 0, j = i*3; k <= 2; j++, k++) {
				poly[k] = delaunayPts[j];
			}
			Polygons.Add(poly);
		}
    }


}
