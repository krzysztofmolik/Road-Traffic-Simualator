using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Arcane.Xna.Presentation {

  internal class UpdateOrderComparer : IComparer<IUpdateable> {
    // Fields
    public static readonly UpdateOrderComparer Default = new UpdateOrderComparer();

    // Methods
    public int Compare(IUpdateable x, IUpdateable y) {
      if((x == null) && (y == null)) {
        return 0;
      }
      if(x != null) {
        if(y == null) {
          return -1;
        }
        if(x.Equals(y)) {
          return 0;
        }
        if(x.UpdateOrder < y.UpdateOrder) {
          return -1;
        }
      }
      return 1;
    }
  }

} // namespace Arcane.Windows.Forms.Xna
