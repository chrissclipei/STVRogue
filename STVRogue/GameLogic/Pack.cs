﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
    public class Pack
    {
        String id;
        public List<Monster> members = new List<Monster>();
        int startingHP = 0;
        public Node location;
        public Dungeon dungeon;

        public Pack(String id, uint n)
        {
            this.id = id;
            for (int i = 0; i < n; i++)
            {
                Monster m = new Monster("" + id + "_" + i);
                members.Add(m);
                startingHP += m.HP;
            }
        }

        public void Attack(Player p)
        {
            foreach (Monster m in members)
            {
                m.Attack(p);
                if (p.HP == 0) break;
            }
        }

        /* Move the pack to an adjacent node. */
        public void move(Node u)
        {
            if (!location.neighbors.Contains(u)) throw new ArgumentException();
			int capacity = (int) (dungeon.M * (dungeon.level(u) + 1));
            // count monsters already in the node:
            foreach (Pack Q in location.packs) {
                capacity = capacity - Q.members.Count;
            }
            // capacity now expresses how much space the node has left
            if (members.Count > capacity)
            {
                Logger.log("Pack " + id + " is trying to move to a full node " + u.id + ", but this would cause the node to exceed its capacity. Rejected.");
                return;
            }
            location = u;
            u.packs.Add(this);
            Logger.log("Pack " + id + " moves to an already full node " + u.id + ". Not rejected.");

        }

        /* Move the pack one node further along a shortest path to u. */
        public void moveTowards(Node u)
        {
			List<Node> path = dungeon.shortestpath(location, u);
            move(path[0]);
        }
        
        /*ADDED*/
        public Monster getMonster() {
            foreach (Monster m in members)
            {
                if (m.HP > 0){
                    return m;
                }
            }
            throw new ArgumentException(); /*this is when no monsters are alive*/
        }

        /*ADDED*/ 
        public int getAction() {
            if (( ((1- ((m.HP)/(m.startingHP))/2))*100) < RandomGenerator.rnd.Next(100)){
                return 2;
            } else {
                return 1; /* A  test in which 1 means attack, 2 means flee*/
            }
        }

        /*ADDED*/ 
        public void flee() {
            /*is there an adjacent node? if so, remove pack, add to other node. To do so, Node class neighbors that is not a bridge*/
            /* Pack.location is the node*/            
            throw new NotImplementedException();
        }
        
    }
}