using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Matrix<TRow, TColumn, TValue>
    {
        private Dictionary<TRow, Dictionary<TColumn, TValue>> InternalDict = new Dictionary<TRow, Dictionary<TColumn, TValue>>();
        private TValue DefaultValue;
        public HashSet<TRow> RowSet => _RowSet;
        public int RowCount => _RowSet.Count;
        private HashSet<TRow> _RowSet = new HashSet<TRow>();
        public HashSet<TColumn> ColumnSet => _ColumnSet;
        public int ColumnCount => _ColumnSet.Count;
        private HashSet<TColumn> _ColumnSet = new HashSet<TColumn>();
        public Matrix(TValue defaultValue)
        {
            DefaultValue = defaultValue;
        }
        public Matrix() { }
            
        public TValue this[TRow r, TColumn c]
        {
            get
            {
                Sanity.Requires(RowSet.Contains(r) && ColumnSet.Contains(c), $"The row-column pair [{r}-{c}] doesn't exist.");
                Sanity.Requires(InternalDict.ContainsKey(r), "Mismatch in key sets.");
                var dict = InternalDict[r];
                return dict.ContainsKey(c) ? dict[c] : DefaultValue;
            }
            set
            {
                if (!RowSet.Contains(r))
                {
                    RowSet.Add(r);
                    InternalDict.Add(r, new Dictionary<TColumn, TValue>());
                }
                if (!ColumnSet.Contains(c))
                {
                    ColumnSet.Add(c);
                }
                InternalDict[r][c] = value;
            }
        }

        public Matrix<TColumn,TRow,TValue> Transpose()
        {
            Matrix<TColumn, TRow, TValue> m = new Matrix<TColumn, TRow, TValue>(DefaultValue);
            foreach(TColumn c in ColumnSet)
            {
                foreach(TRow r in RowSet)
                {
                    m[c, r] = this[r, c];
                }
            }
            return m;
        }

        public IEnumerable<string> Output(bool withColumnHeader = false, bool withRowHeader = false)
        {
            if (withColumnHeader)
            {
                if (withRowHeader)
                    yield return string.Join("\t", "".Concat(_ColumnSet.Select(x => x.ToString())));
                else
                    yield return string.Join("\t", _ColumnSet);
            }
            foreach(var row in _RowSet)
            {
                if (withRowHeader)
                {
                    yield return string.Join("\t", row.ToString().Concat(DeDemensionRow(row).Select(x => x.ToString())));
                }
                else
                {
                    yield return string.Join("\t", DeDemensionRow(row));
                }
            }
        }

        public IEnumerable<TValue> DeDemensionColumn(TColumn c)
        {
            foreach (var r in _RowSet)
                yield return this[r, c];
        }
        public IEnumerable<TValue> DeDemensionRow(TRow r)
        {
            foreach (var c in _ColumnSet)
                yield return this[r, c];
        }
    }
}
