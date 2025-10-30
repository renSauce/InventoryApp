using System;

namespace InventoryApp.Robotics
{
    public class ItemSorterRobot : Robot
    {
        public const string UrscriptTemplate = @"
def move_item_to_shipment_box():
  Z_UP   = 0.25
  Z_DOWN = 0.12
  ITEM_Y = -0.40
  BOX_X  = 0.20
  BOX_Y  = -0.40

  RX = 0
  RY = d2r(180)
  RZ = 0

  def pose(x, y, z):
    return p[x, y, z, RX, RY, RZ]
  end

  def go(p):
    movej(get_inverse_kin(p))
  end

  # item X position based on slot index
  ITEM_X = ({0} - 2) * 0.15  # 1→-0.15, 2→0.0, 3→+0.15

  home = pose(0, -0.20, Z_UP)
  at_item = pose(ITEM_X, ITEM_Y, Z_DOWN)
  above_item = pose(ITEM_X, ITEM_Y, Z_UP)
  at_box = pose(BOX_X, BOX_Y, Z_DOWN)
  above_box = pose(BOX_X, BOX_Y, Z_UP)

  go(home)
  go(above_item)
  go(at_item)
  sleep(0.3)
  go(above_item)
  go(above_box)
  go(at_box)
  sleep(0.3)
  go(above_box)
  go(home)
end

move_item_to_shipment_box()
";

        public void PickUp(uint itemIndex)
        {
            var program = string.Format(UrscriptTemplate, itemIndex);
            SendUrscript(program);
        }
    }
}
