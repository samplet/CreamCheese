using System;
using System.Collections.Generic;

namespace Excream {

    class ReferenceIdKeeper<T> {

        private int _nextAvailableId;
        private Dictionary<int, T> _ids;

        public ReferenceIdKeeper() {
            _nextAvailableId = 0;
            _ids = new Dictionary<int, T>();
        }

        public void Add(T ob) {
            _ids.Add(_nextAvailableId, ob);
            _nextAvailableId++;
        }

        public bool Remove(int ob) {
            return _ids.Remove(ob);
        }

        public int this[T ob] {
            get {
                foreach(KeyValuePair<int, T> idsKvp in _ids) {
                    if(object.ReferenceEquals(ob, idsKvp.Value)) {
                        return idsKvp.Key;
                    }
                }
                throw new KeyNotFoundException();
            }
        }

        public T this[int id] {
            get {
                return _ids[id];
            }
        }

    }

}
