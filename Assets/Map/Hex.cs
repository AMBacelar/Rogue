using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Hex class defines the grid position, world space position, size, 
/// neighbours, etc... of a Hex Tile. However, it does NOT interact with
/// Unity directly in any way.
/// </summary>
public class Hex
{
    public static readonly Hex Zero = new Hex(0, 0, 0);
    public static readonly Hex NorthEast = new Hex(-1, 0, 1);
    public static readonly Hex NorthWest = new Hex(-1, 1, 0);
    public static readonly Hex SouthEast = new Hex(1, -1, 0);
    public static readonly Hex SouthWest = new Hex(1, 0, -1);
    public static readonly Hex East = new Hex(0, -1, 1);
    public static readonly Hex West = new Hex(0, 1, -1);

    #region Constructors

    public Hex(bool _isWalkable, int _tileType, int q, int r)
    {
        this.tileType = _tileType;
        this.isWalkable = _isWalkable;
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }
    public Hex(bool _isWalkable, int _tileType, int q, int r, int s)
        : this(_isWalkable, _tileType, q, r)
    {
        this.S = s;
        if (q + r + s != 0) throw new ArgumentException("q+r+s must equal zero!");
    }

    public Hex(int q, int r)
    {
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }
    public Hex(int q, int r, int s)
        : this(q, r)
    {
        this.S = s;
        if (q + r + s != 0) throw new ArgumentException("q+r+s must equal zero!");
    }
    // Q + R + S = 0
    // S = -(Q + R)

    #endregion

    public int Q;  // Column
    public int R;  // Row
    public int S;
    public bool isWalkable;
    public int tileType;

    #region Class Interface

    public static bool operator ==(Hex a, Hex b)
    {
        return a.Q == b.Q && a.R == b.R && a.S == b.S;
    }

    public static bool operator !=(Hex a, Hex b)
    {
        return !(a == b);
    }

    public static Hex hex_add(Hex a, Hex b)
    {
        return new Hex(a.isWalkable, a.tileType, a.Q + b.Q, a.R + b.R, a.S + b.S);
    }

    public static Hex hex_subtract(Hex a, Hex b)
    {
        return new Hex(a.isWalkable, a.tileType, a.Q - b.Q, a.R - b.R, a.S - b.S);
    }

    public static Hex hex_multiply(Hex a, int k)
    {
        return new Hex(a.isWalkable, a.tileType, a.Q * k, a.R * k, a.S * k);
    }

    public static int Distance(Hex lhs, Hex rhs)
    {
        var doubleDistance =
            Math.Abs(lhs.Q - rhs.Q) +
            Math.Abs(lhs.R - rhs.R) +
            Math.Abs(lhs.S - rhs.S);

        return doubleDistance / 2;
    }

    Hex[] neighbours;

    public Hex[] GetNeighbours()
    {
        if (this.neighbours != null)
            return this.neighbours;

        List<Hex> neighbours = new List<Hex>();

        neighbours.Add(hex_add(this, NorthEast));
        neighbours.Add(hex_add(this, NorthWest));
        neighbours.Add(hex_add(this, SouthEast));
        neighbours.Add(hex_add(this, SouthWest));
        neighbours.Add(hex_add(this, East));
        neighbours.Add(hex_add(this, West));

        this.neighbours = neighbours.ToArray();

        return this.neighbours;
    }

    #endregion

    #region Overrides

    public override int GetHashCode()
    {
        return ((Q * 17) << 8) * ((R * 29) << 4) * S;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Hex))
            return false;

        return Equals((Hex)obj);
    }

    public override string ToString()
    {
        return "Hex - Q: " + Q + " R: " + R + " S: " + S;
    }

    #endregion
}