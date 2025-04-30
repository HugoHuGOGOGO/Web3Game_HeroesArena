using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using DG.Tweening;
using Dynamitey.DynamicObjects;
using Nethereum.Web3;
using UnityEngine;
using Thirdweb;
using Thirdweb.AccountAbstraction;

using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms;
using System.Runtime.InteropServices;



public class web3Manager : MonoBehaviour
{
	[Preserve]
	[Serializable]
	public class UserData
	{
		[Preserve]
		public string gold = "10000";
		[Preserve]
		public string diamond;
		[Preserve]
		public string ladderRating = "1000";
	}
	public UserData userdata = new UserData();
	public static web3Manager Instance { get; private set; }
	private ThirdwebSDK sdk;
	private string userManager_Address = "0x885890C94e00E517c05455427a9e960015640Ab6";
	private string diamond_Address = "0xAb2588914c40cA53E3053f1847D4F816891Ca4F0";
  private bool hasPendingTx = false;
	private string timer_Address = "0x4e51f3aF87b8A91659A3Ac3ce8a9B3cD057d09A9";
	private string nftManager_Address = "0x4706fe6F548da2D0Bd6D5b61d31A44aCa8DEAdC4";
	// public ulong diamond;
	// public ulong gold;
	// public ulong ladderRating;
	private string customerAddress;
    private readonly Queue<Func<Task>> contractCallQueue = new Queue<Func<Task>>();
    private bool isExecuting = false;
	
    
	

	// Start is called before the first frame update
	private string userManager_abi = @"[
  {
    ""inputs"": [{ ""internalType"": ""address"", ""name"": ""user"", ""type"": ""address"" }],
    ""name"": ""isNew"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      { ""internalType"": ""address"", ""name"": ""user"", ""type"": ""address"" },
      { ""internalType"": ""uint256"", ""name"": ""amount"", ""type"": ""uint256"" }
    ],
    ""name"": ""setDiamond"",
    ""outputs"": [{ ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" }],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      { ""internalType"": ""address"", ""name"": ""user"", ""type"": ""address"" },
      { ""internalType"": ""uint256"", ""name"": ""amount"", ""type"": ""uint256"" }
    ],
    ""name"": ""setGold"",
    ""outputs"": [{ ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" }],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      { ""internalType"": ""address"", ""name"": ""user"", ""type"": ""address"" },
      { ""internalType"": ""uint256"", ""name"": ""rating"", ""type"": ""uint256"" }
    ],
    ""name"": ""setLadderRating"",
    ""outputs"": [{ ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" }],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      { ""internalType"": ""address"", ""name"": ""user"", ""type"": ""address"" },
      { ""internalType"": ""uint256"", ""name"": ""amount"", ""type"": ""uint256"" }
    ],
    ""name"": ""useDiamond"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""DEFAULT_DIAMOND"",
    ""outputs"": [{ ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" }],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""DEFAULT_GOLD"",
    ""outputs"": [{ ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" }],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""DEFAULT_LADDER_RATING"",
    ""outputs"": [{ ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" }],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [{ ""internalType"": ""address"", ""name"": ""user"", ""type"": ""address"" }],
    ""name"": ""getUserData"",
    ""outputs"": [
      { ""internalType"": ""string"", ""name"": ""gold"", ""type"": ""string"" },
      { ""internalType"": ""string"", ""name"": ""diamond"", ""type"": ""string"" },
      { ""internalType"": ""string"", ""name"": ""ladderRating"", ""type"": ""string"" }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [{ ""internalType"": ""address"", ""name"": ""user"", ""type"": ""address"" }],
    ""name"": ""ifNew"",
    ""outputs"": [{ ""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256"" }],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  }
]";

	string diamond_abi = @"[
    {
        ""inputs"": [
            {
                ""internalType"": ""uint256"",
                ""name"": ""tier"",
                ""type"": ""uint256""
            }
        ],
        ""name"": ""purchaseDiamonds"",
        ""outputs"": [
            {
                ""internalType"": ""bool"",
                ""name"": """",
                ""type"": ""bool""
            }
        ],
        ""stateMutability"": ""payable"",
        ""type"": ""function""
    },
    {
        ""inputs"": [
            {
                ""internalType"": ""address"",
                ""name"": ""_userManager"",
                ""type"": ""address""
            }
        ],
        ""stateMutability"": ""nonpayable"",
        ""type"": ""constructor""
    },
    {
        ""inputs"": [],
        ""name"": ""withdraw"",
        ""outputs"": [],
        ""stateMutability"": ""nonpayable"",
        ""type"": ""function""
    },
    {
        ""inputs"": [],
        ""name"": ""owner"",
        ""outputs"": [
            {
                ""internalType"": ""address"",
                ""name"": """",
                ""type"": ""address""
            }
        ],
        ""stateMutability"": ""view"",
        ""type"": ""function""
    },
    {
        ""inputs"": [],
        ""name"": ""UNIT_PRICE"",
        ""outputs"": [
            {
                ""internalType"": ""uint256"",
                ""name"": """",
                ""type"": ""uint256""
            }
        ],
        ""stateMutability"": ""view"",
        ""type"": ""function""
    },
    {
        ""inputs"": [],
        ""name"": ""userManager"",
        ""outputs"": [
            {
                ""internalType"": ""contract IUserManager"",
                ""name"": """",
                ""type"": ""address""
            }
        ],
        ""stateMutability"": ""view"",
        ""type"": ""function""
    }
]";

	
    string timer_abi = @"[
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""user"",
        ""type"": ""address""
      }
    ],
    ""name"": ""addNumber"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""user"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""index"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""deleteNumber"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""user"",
        ""type"": ""address""
      }
    ],
    ""name"": ""queryList"",
    ""outputs"": [
      {
        ""internalType"": ""string[4]"",
        ""name"": """",
        ""type"": ""string[4]""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  }
]";
    private string nftManager_abi = @"[
  {
    ""inputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""constructor""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""sender"", ""type"": ""address""},
      {""internalType"": ""uint256"", ""name"": ""balance"", ""type"": ""uint256""},
      {""internalType"": ""uint256"", ""name"": ""needed"", ""type"": ""uint256""},
      {""internalType"": ""uint256"", ""name"": ""tokenId"", ""type"": ""uint256""}
    ],
    ""name"": ""ERC1155InsufficientBalance"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""approver"", ""type"": ""address""}
    ],
    ""name"": ""ERC1155InvalidApprover"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {""internalType"": ""uint256"", ""name"": ""idsLength"", ""type"": ""uint256""},
      {""internalType"": ""uint256"", ""name"": ""valuesLength"", ""type"": ""uint256""}
    ],
    ""name"": ""ERC1155InvalidArrayLength"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""operator"", ""type"": ""address""}
    ],
    ""name"": ""ERC1155InvalidOperator"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""receiver"", ""type"": ""address""}
    ],
    ""name"": ""ERC1155InvalidReceiver"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""sender"", ""type"": ""address""}
    ],
    ""name"": ""ERC1155InvalidSender"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""operator"", ""type"": ""address""},
      {""internalType"": ""address"", ""name"": ""owner"", ""type"": ""address""}
    ],
    ""name"": ""ERC1155MissingApprovalForAll"",
    ""type"": ""error""
  },
  {
    ""anonymous"": false,
    ""inputs"": [
      {""indexed"": true, ""internalType"": ""address"", ""name"": ""account"", ""type"": ""address""},
      {""indexed"": true, ""internalType"": ""address"", ""name"": ""operator"", ""type"": ""address""},
      {""indexed"": false, ""internalType"": ""bool"", ""name"": ""approved"", ""type"": ""bool""}
    ],
    ""name"": ""ApprovalForAll"",
    ""type"": ""event""
  },
  {
    ""anonymous"": false,
    ""inputs"": [
      {""indexed"": true, ""internalType"": ""address"", ""name"": ""operator"", ""type"": ""address""},
      {""indexed"": true, ""internalType"": ""address"", ""name"": ""from"", ""type"": ""address""},
      {""indexed"": true, ""internalType"": ""address"", ""name"": ""to"", ""type"": ""address""},
      {""indexed"": false, ""internalType"": ""uint256[]"", ""name"": ""ids"", ""type"": ""uint256[]""},
      {""indexed"": false, ""internalType"": ""uint256[]"", ""name"": ""values"", ""type"": ""uint256[]""}
    ],
    ""name"": ""TransferBatch"",
    ""type"": ""event""
  },
  {
    ""anonymous"": false,
    ""inputs"": [
      {""indexed"": true, ""internalType"": ""address"", ""name"": ""operator"", ""type"": ""address""},
      {""indexed"": true, ""internalType"": ""address"", ""name"": ""from"", ""type"": ""address""},
      {""indexed"": true, ""internalType"": ""address"", ""name"": ""to"", ""type"": ""address""},
      {""indexed"": false, ""internalType"": ""uint256"", ""name"": ""id"", ""type"": ""uint256""},
      {""indexed"": false, ""internalType"": ""uint256"", ""name"": ""value"", ""type"": ""uint256""}
    ],
    ""name"": ""TransferSingle"",
    ""type"": ""event""
  },
  {
    ""anonymous"": false,
    ""inputs"": [
      {""indexed"": false, ""internalType"": ""string"", ""name"": ""value"", ""type"": ""string""},
      {""indexed"": true, ""internalType"": ""uint256"", ""name"": ""id"", ""type"": ""uint256""}
    ],
    ""name"": ""URI"",
    ""type"": ""event""
  },
  {
    ""inputs"": [],
    ""name"": ""NFT_TYPE_COUNT"",
    ""outputs"": [{""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256""}],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""account"", ""type"": ""address""},
      {""internalType"": ""uint256"", ""name"": ""id"", ""type"": ""uint256""}
    ],
    ""name"": ""balanceOf"",
    ""outputs"": [{""internalType"": ""uint256"", ""name"": """", ""type"": ""uint256""}],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {""internalType"": ""address[]"", ""name"": ""accounts"", ""type"": ""address[]""},
      {""internalType"": ""uint256[]"", ""name"": ""ids"", ""type"": ""uint256[]""}
    ],
    ""name"": ""balanceOfBatch"",
    ""outputs"": [{""internalType"": ""uint256[]"", ""name"": """", ""type"": ""uint256[]""}],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""player"", ""type"": ""address""}
    ],
    ""name"": ""distributeAllNFTs"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""player"", ""type"": ""address""},
      {""internalType"": ""uint256"", ""name"": ""t1"", ""type"": ""uint256""},
      {""internalType"": ""uint256"", ""name"": ""t2"", ""type"": ""uint256""},
      {""internalType"": ""uint256"", ""name"": ""t3"", ""type"": ""uint256""},
      {""internalType"": ""uint256"", ""name"": ""t4"", ""type"": ""uint256""}
    ],
    ""name"": ""distributeNFTsByTypes"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""account"", ""type"": ""address""},
      {""internalType"": ""address"", ""name"": ""operator"", ""type"": ""address""}
    ],
    ""name"": ""isApprovedForAll"",
    ""outputs"": [{""internalType"": ""bool"", ""name"": """", ""type"": ""bool""}],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {""internalType"": ""address"", ""name"": ""player"", ""type"": ""address""}
    ],
    ""name"": ""queryPlayerNFTs"",
    ""outputs"": [{""internalType"": ""string[]"", ""name"": """", ""type"": ""string[]""}],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  }
]";


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject); // 防止重复保留
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
#if UNITY_WEBGL && !UNITY_EDITOR
        SetPendingTx(false);
#endif
        
	}

	void Start()
	{
		sdk = ThirdwebManager.Instance.SDK;

	
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void EnqueueContractCall(Func<Task> call)
	{
	    contractCallQueue.Enqueue(call);
	    if (!isExecuting)
	        _ = ProcessQueue();
	}
	
	private async Task ProcessQueue()
	{
	    isExecuting = true;
	    while (contractCallQueue.Count > 0)
	    {
	        var nextCall = contractCallQueue.Dequeue();
	
#if UNITY_WEBGL && !UNITY_EDITOR
	        SetPendingTx(true);
#endif
	        try
	        {
	            await nextCall();  // ✅ 确保上一个执行完，才会执行下一个
	        }
	        catch (Exception ex)
	        {
	            Debug.LogError("Contract call failed: " + ex.Message);
	        }
#if UNITY_WEBGL && !UNITY_EDITOR
	        SetPendingTx(false);
#endif
	    }
	    isExecuting = false;
	}
	
	[DllImport("__Internal")]
	private static extern void SetPendingTx(bool hasPending);

	public async void GetCustomerAddress()
	{
		customerAddress = await sdk.Wallet.GetAddress();
	}

    
	public async void Register()
	{
		
		customerAddress = await sdk.Wallet.GetAddress();
		Contract contract = sdk.GetContract(userManager_Address, userManager_abi);
        BigInteger data = await contract.Read<BigInteger>("ifNew", new object[] { customerAddress });
        if (data == 0)
        {
            await firstNFT();
            await contract.Write("isNew", new object[] { customerAddress });

        }
        await getChestTimer();
        await getUserData();
        await getNFTInfo();
        if(SceneManager.GetActiveScene().buildIndex != 1)
        {
            // 在场景切换前调用（例如 web3Manager 中）
            SceneTracker.PreviousSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("MainScene"); // 场景1
            //SceneManager.LoadScene(1);
        }
	}

	public async Task getUserData()
	{
		customerAddress = await sdk.Wallet.GetAddress();
		Contract contract = sdk.GetContract(userManager_Address, userManager_abi);
		var result = await contract.Read<List<string>>("getUserData", new object[] { customerAddress });
		userdata.gold = result[0];
		userdata.diamond = result[1];
		userdata.ladderRating = result[2];


        
		
	}

	

	public async void DiamondPurchase(int id)
	{
        EnqueueContractCall(async () =>
        {

        customerAddress = await sdk.Wallet.GetAddress();
        Contract contract = sdk.GetContract(diamond_Address, diamond_abi);
		
        // 钻石数量与 tier 对应关系
        Dictionary<int, int> tierToCount = new Dictionary<int, int>()
        {
            {1, 1},
            {2, 6},
            {3, 30},
            {4, 128},
            {5, 328},
            {6, 648}
        };

        if (!tierToCount.ContainsKey(id))
        {
            Debug.LogError("Invalid diamond purchase tier selected.");
            return;
        }

        // 单价 0.0007 ETH（单位为 wei）
        decimal unitPriceInEth = 0.0007m;
        decimal totalPrice = unitPriceInEth * tierToCount[id];

        try
        {
            var overrides = new TransactionRequest
            {
                value = Web3.Convert.ToWei(totalPrice).ToString()
            };
            var data = await contract.Write("purchaseDiamonds", overrides, id);
            if (data.receipt.status == 1)
            {
	            userdata.diamond = (int.Parse(userdata.diamond) + tierToCount[id]).ToString();
                ChioceUIManager.Instance.panel_waiting.SetActive(false);
                int currentDiamonds = int.TryParse(ChioceUIManager.Instance.diamandText.text, out var parsed) ? parsed : 0;
                ChioceUIManager.Instance.diamandText.text = (currentDiamonds + tierToCount[id]).ToString();                
                ChioceUIManager.Instance.SetTSorTF(true);
            }
            else
            {
                ChioceUIManager.Instance.panel_waiting.SetActive(false);
                ChioceUIManager.Instance.SetTSorTF(false);
            }
        }
        catch (System.Exception ex)
        {
            ChioceUIManager.Instance.panel_waiting.SetActive(false);
            ChioceUIManager.Instance.SetTSorTF(false);
            Debug.LogError("Diamond purchase failed: " + ex.Message);
        }
        });
    }

    public async void SetGoldAndRate()
    {
        EnqueueContractCall(async () =>
        {
    
            customerAddress = await sdk.Wallet.GetAddress();
            Contract contract = sdk.GetContract(userManager_Address, userManager_abi);
            var data1 = await contract.Write("setGold", new object[] { customerAddress, 800});
            var data2 = await contract.Write("setLadderRating", new object[] { customerAddress, 12 });
        });
    }

    public async Task  getChestTimer()
    {
        customerAddress = await sdk.Wallet.GetAddress();
        Contract contract = sdk.GetContract(timer_Address, timer_abi);
        var data = await contract.Read<List<string>>("queryList" , customerAddress );
        Chest_Manager.Instance.chestList[0] = long.Parse(data[0]);
        Chest_Manager.Instance.chestList[1] = long.Parse(data[1]);
        Chest_Manager.Instance.chestList[2] = long.Parse(data[2]);
        Chest_Manager.Instance.chestList[3] = long.Parse(data[3]);
    }
    
    public async void SetChestTimer()
    {
        EnqueueContractCall(async () =>
        {
            
            customerAddress = await sdk.Wallet.GetAddress();
            Contract contract = sdk.GetContract(timer_Address, timer_abi);
            var data = await contract.Write("addNumber", new object[] { customerAddress });
        });
    }
    
    public async void useDiamond(int amount )
	{
        
        EnqueueContractCall(async () =>
        {
            
            customerAddress = await sdk.Wallet.GetAddress();
            Contract contract = sdk.GetContract(userManager_Address, userManager_abi);
            var data = await contract.Write("useDiamond", new object[] { customerAddress, amount });
        });
	}

	public async void deleteChestTimer(int index)
	{
        EnqueueContractCall(async () =>
        {
            
            customerAddress = await sdk.Wallet.GetAddress();
            Contract contract = sdk.GetContract(timer_Address, timer_abi);
            var data = await contract.Write("deleteNumber", new object[] { customerAddress, index });
        });
	}

	public async Task firstNFT()
	{
		customerAddress = await sdk.Wallet.GetAddress();
		Contract contract = sdk.GetContract(nftManager_Address, nftManager_abi);
		var data = await contract.Write("distributeAllNFTs", new object[] { customerAddress });
	}

    public async Task getNFTInfo()
    {
        customerAddress = await sdk.Wallet.GetAddress();
        Contract contract = sdk.GetContract(nftManager_Address, nftManager_abi);
        var data = await contract.Read<List<string>>("queryPlayerNFTs", customerAddress );
        for (int i = 0; i < 12; i++)
        {
            Chest_Manager.Instance.NFTList[i] = int.Parse(data[i]);
        }
        
    }

    public async void updataNFT(int a,int b,int c,int d)
    {
        EnqueueContractCall(async () =>
        {
            customerAddress = await sdk.Wallet.GetAddress();
            Contract contract = sdk.GetContract(nftManager_Address, nftManager_abi);
            var data = await contract.Write("distributeNFTsByTypes", new object[] { customerAddress, a, b, c, d });
        });
    }
}
//EnqueueContractCall(async () =>
//{
    
//});
	
