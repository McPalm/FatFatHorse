using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToken
{
    public bool Jump => Time.realtimeSinceStartup < _jumpTime;
    public bool Attack => Time.realtimeSinceStartup < _attackTime;
    public bool Block => HoldBlock;
    public bool Special => Time.realtimeSinceStartup < _specialTime;
    public float Horizontal { get; private set; }
    public float AbsHor => Mathf.Abs(Horizontal);
    public float Vertical { get; private set; }
    public float AbsVer => Mathf.Abs(Vertical);

    public float Buffer { set; get; } = .15f;

    public void PressJump() => _jumpTime = Time.realtimeSinceStartup + Buffer;
    public void PressAttack() => _attackTime = Time.realtimeSinceStartup + Buffer;
    public void PressSpecial() => _specialTime = Time.realtimeSinceStartup + Buffer;
    public bool HoldBlock { get; set; }
    public bool HoldJump { get; set; }

    public void ClearJump() => _jumpTime = -12f;
    public void ClearAttack() => _attackTime = -12f;
    public void ClearSpecial() => _specialTime = -12f;


    private float _jumpTime = -12f;
    private float _attackTime = -12f;
    private float _specialTime = -12f;

    Vector2Int[] InputHistory = new Vector2Int[10];
    int count = 0;
    float lastHistoryEntry;
    bool ForceEntry => Time.timeSinceLevelLoad > lastHistoryEntry + .3f;

    public Vector2Int CurrentDirection => InputHistory[count];

    public void SetDirection(Vector2 dir)
    {
        var norm = new Vector2Int(
            Mathf.RoundToInt(dir.x),
            Mathf.RoundToInt(dir.y)
            );
        if (norm != CurrentDirection || ForceEntry)
        {
            count++;
            count %= 10;
            InputHistory[count] = norm;
            lastHistoryEntry = Time.timeSinceLevelLoad;
        }
        Horizontal = dir.x;
        Vertical = dir.y;
    }

    public List<Vector2Int> DirectionHistory(int inputs)
    {
        inputs = inputs > 10 ? 10 : inputs;
        var list = new List<Vector2Int>();
        for (int i = 0; i < inputs; i++)
        {
            int wnat = count - inputs + i + 1;
            wnat = (wnat + 10) % 10;

            list.Add(InputHistory[wnat]);
        }
        return list;
    }

    public bool DP(int direction)
    {
        if (direction < 0)
        {
            return MatchSequence(new Vector2Int[]
                {
                Vector2Int.left,
                Vector2Int.down,
                new Vector2Int(-1, -1),
                },1);
        }
        return MatchSequence(new Vector2Int[]
                {
                Vector2Int.right,
                Vector2Int.down,
                new Vector2Int(1, -1),
                },1);
    }

    public bool HalfRoll(int direction)
    {
        if (direction != CurrentDirection.x)
            return false;
        int sequence = 0;
        foreach (var input in DirectionHistory(4))
        {
            if (sequence == 0 && input.x == -direction)
                sequence++;
            else if (sequence == 1 && input.y == -1)
                sequence++;
            if (sequence == 2 && input.x == direction)
                return true;
        }
        return false;
    }

    public bool MatchSequence(Vector2Int[] inputs, int allowedMissInputs = 1)
    {
        int matches = 0;
        var history = DirectionHistory(inputs.Length + allowedMissInputs);
        if (inputs[inputs.Length - 1] != history[history.Count - 1])
            return false;
        foreach (Vector2Int input in history)
        {
            Vector2Int fuck = inputs[matches];
            if(input == fuck)
            {
                matches++;
                if (matches == inputs.Length)
                    return true;
            }
        }
        return false;
    }

    public bool Charge(Vector2Int Direction)
    {
        var history = DirectionHistory(4);
        if (Direction.x == 0)
        {
            return history[0].y == Direction.y
                && history[1].y == Direction.y
                && history[2].y == Direction.y
                && history[3].y == Direction.y;
        }
        return history[0].x == Direction.x
                && history[1].x == Direction.x
                && history[2].x == Direction.x
                && history[3].x == Direction.x;
    }
}
