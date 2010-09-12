using System.Runtime.InteropServices;

namespace Excream {

    [Guid("F34FDDCB-2C0F-447b-A50B-11BA75051F14")]
    [ComVisible(true)]
    public interface IFunctions {
        /*
         * This would be the function header if circular references could be
         * better managed:
         * object CONSTRAIN(object constraint1, [Optional] object constraint2, [Optional] object constraint3,
         *            [Optional] object constraint4, [Optional] object constraint5, [Optional] object constraint6,
         *            [Optional] object constraint7, [Optional] object constraint8, [Optional] object constraint9,
         *            [Optional] object constraint10, [Optional] object constraint11, [Optional] object constraint12,
         *            [Optional] object constraint13, [Optional] object constraint14, [Optional] object constraint15,
         *            [Optional] object constraint16, [Optional] object constraint17, [Optional] object constraint18,
         *            [Optional] object constraint19, [Optional] object constraint20, [Optional] object constraint21,
         *            [Optional] object constraint22, [Optional] object constraint23, [Optional] object constraint24,
         *            [Optional] object constraint25, [Optional] object constraint26, [Optional] object constraint27,
         *            [Optional] object constraint28, [Optional] object constraint29, [Optional] object constraint30,
         *            [Optional] object constraint31, [Optional] object constraint32);
         */
        object CONSTRAIN(string constraints);
    }

}
