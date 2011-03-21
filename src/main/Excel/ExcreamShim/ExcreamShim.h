

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0555 */
/* at Mon Mar 21 02:52:33 2011
 */
/* Compiler settings for ExcreamShim.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 7.00.0555 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__


#ifndef __ExcreamShim_h__
#define __ExcreamShim_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ConnectProxy_FWD_DEFINED__
#define __ConnectProxy_FWD_DEFINED__

#ifdef __cplusplus
typedef class ConnectProxy ConnectProxy;
#else
typedef struct ConnectProxy ConnectProxy;
#endif /* __cplusplus */

#endif 	/* __ConnectProxy_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 



#ifndef __ExcreamShimLib_LIBRARY_DEFINED__
#define __ExcreamShimLib_LIBRARY_DEFINED__

/* library ExcreamShimLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_ExcreamShimLib;

EXTERN_C const CLSID CLSID_ConnectProxy;

#ifdef __cplusplus

class DECLSPEC_UUID("f4389bd9-a9c7-496f-926a-bd8730a43a8a")
ConnectProxy;
#endif
#endif /* __ExcreamShimLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


