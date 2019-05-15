using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player
{
    public string name { get; private set; }
    public int identity { get; private set; }
    public int position { get; private set; }

    public Player(string name, int position, int identity)
    {
        this.name = name;
        this.identity = identity;
        this.position = position;
    }
}

