mergeInto(LibraryManager.library, {
  SetPendingTx: function (hasPending) {
    if (hasPending) {
      console.log("Transaction pending...");
      // 显示提示框或修改 DOM 都可以
    } else {
      console.log("No pending transaction.");
    }
  }
});