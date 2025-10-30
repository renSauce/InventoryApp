using System;
using System.Globalization;

namespace InventoryApp.Robotics
{
    public class ItemSorterRobot : Robot
    {
        public const string UrscriptTemplate = @"
def move_item_to_shipment_box():
  # OnRobot XML-RPC
  CONTROL_BOX_IP = ""{1}""
  global RPC = rpc_factory(""xmlrpc"", ""http://"" + CONTROL_BOX_IP + "":41414"")
  global TOOL_INDEX = 0

  def rg_is_busy():
    return RPC.rg_get_busy(TOOL_INDEX)
  end

  def rg_grip(width, force=20):
    RPC.rg_grip(TOOL_INDEX, width + .0, force + .0)
    sleep(0.01)
    while (rg_is_busy()):
      sync()
    end
  end

  # Coordinates from Week 7.
  Z_UP = 0.25
  Z_DOWN = 0.12
  ITEM_Y = -0.40
  BOX_X = 0.20
  BOX_Y = -0.40
  RX = 0
  RY = d2r(180)
  RZ = 0

  def pose(x, y, z): return p[x, y, z, RX, RY, RZ] end
  def go(p): movej(get_inverse_kin(p)) end

  ITEM_X = ({0} - 2) * 0.15   # 1→-0.15, 2→0.0, 3→+0.15

  home = pose(0, -0.20, Z_UP)
  at_item = pose(ITEM_X, ITEM_Y, Z_DOWN)
  above_item = pose(ITEM_X, ITEM_Y, Z_UP)
  at_box = pose(BOX_X, BOX_Y, Z_DOWN)
  above_box = pose(BOX_X, BOX_Y, Z_UP)

  # Grip params
  OPEN_W=50; OPEN_F=15; CLOSE_W=0; CLOSE_F=25

  go(above_item)
  rg_grip(OPEN_W, OPEN_F)
  go(at_item)
  rg_grip(CLOSE_W, CLOSE_F)
  go(above_item)

  go(above_box)
  go(at_box)
  rg_grip(OPEN_W, OPEN_F)
  go(above_box)
end

move_item_to_shipment_box()
";

        public void PickUp(uint itemIndex)
        {
            var program = string.Format(
                CultureInfo.InvariantCulture,
                UrscriptTemplate,
                itemIndex,
                ControlBoxIpAddress   // <- comes from GUI
            );

            SendUrscript(program);
        }
    }
}
