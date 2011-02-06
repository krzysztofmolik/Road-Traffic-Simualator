using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Arcane.Xna.Presentation {

  internal class DrawOrderComparer : IComparer<IDrawable> {
    // Fields
    public static readonly DrawOrderComparer Default = new DrawOrderComparer();

    // Methods
    public int Compare(IDrawable x, IDrawable y) {
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
        if(x.DrawOrder < y.DrawOrder) {
          return -1;
        }
      }
      return 1;
    }
  }

} // namespace Arcane.Windows.Forms.Xna
