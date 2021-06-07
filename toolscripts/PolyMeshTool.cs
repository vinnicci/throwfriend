using Godot;

[Tool]
public class PolyMeshTool : Polygon2D
{
    bool autoPoly;
    [Export] bool AutoPoly {
        get {
            return autoPoly;
        }
        set {
            if(freeformDeform == true && value == true) {
                return;
            }
            autoPoly = value;
            currentPolyCount = Polygon.Length;
            if(autoPoly == true) {
                SetProcess(true);
            }
            else {
                SetProcess(false);
            }
        }
    }
    bool freeformDeform;
    [Export] bool FreeformDeform {
        get {
            return freeformDeform;
        }
        set {
            if((autoPoly == true && value == true) || Polygon.Length < 3) {
                return;
            }
            freeformDeform = value;
            if(freeformDeform == true) {
                if(IsInstanceValid(root) == false) {
                    root = GetTree().EditedSceneRoot;
                }
                SetVertices();
                SetProcess(true);
            }
            else {
                ClearVertices();
                SetProcess(false);
            }
        }
    }


    public override void _Ready()
    {
        base._Ready();
        root = GetTree().EditedSceneRoot;
        AutoPoly = AutoPoly;
        FreeformDeform = FreeformDeform;
    }


    int currentPolyCount;
    Godot.Collections.Array vertices;


    public override void _Process(float delta)
    {
        base._Process(delta);
        if(AutoPoly == true) {
            if(currentPolyCount != Polygon.Length) {
                CreatePoly();
                currentPolyCount = Polygon.Length;
            }
        }
        else if(FreeformDeform == true) {
            Vector2[] polyArr = Polygon;
            for(int i = 0; i <= Polygon.Length - 1; i++) {
                polyArr[i] = ((Position2D)vertices[i]).GlobalPosition;
            }
            Polygon = polyArr;
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


    Node2D verticesNode;


    void SetVertices() {
        ClearVertices();
        verticesNode = new Node2D();
        verticesNode.Name = "Vertices";
        verticesNode.SetMeta("_edit_lock_", true);
        AddChild(verticesNode);
        verticesNode.Owner = root;
        for(int i = 0; i <= Polygon.Length - 1; i++) {
            InstancePosition2DVert(i);
        }
        vertices = verticesNode.GetChildren();
    }


    Node root;


    void InstancePosition2DVert(int idx) {
        Position2D pv = new Position2D();
        pv.GlobalPosition = Polygon[idx];
        pv.Name = idx.ToString();
        verticesNode.AddChild(pv);
        pv.Owner = root;
    }


    void ClearVertices() {
		//cleans up every child, extra careful if polygon2d has children
        foreach(Node2D child in GetChildren()) {
            child.Owner = default;
            child.GetParent().RemoveChild(child);
            child.QueueFree();
        }
        if(vertices != null) {
            vertices.Clear();
        }
    }


}
