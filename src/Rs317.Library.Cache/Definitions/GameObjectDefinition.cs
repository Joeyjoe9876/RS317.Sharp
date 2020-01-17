using System;

namespace Rs317.Sharp
{

	public class GameObjectDefinition
	{
		public static GameObjectDefinition getDefinition(int objectId)
		{
			for(int c = 0; c < 20; c++)
			{
				if(cache[c].id == objectId)
				{
					return cache[c];
				}
			}

			cacheIndex = (cacheIndex + 1) % 20;
			GameObjectDefinition definition = cache[cacheIndex];
			stream.position = streamOffsets[objectId];
			definition.id = objectId;
			definition.setDefaults();
			definition.loadDefinition(stream);
			return definition;
		}

		public static void load(Archive archive)
		{
			GameObjectDefinition.stream = new Default317Buffer(archive.decompressFile("loc.dat"));
			Default317Buffer indexStream = new Default317Buffer(archive.decompressFile("loc.idx"));
			int objectCount = indexStream.getUnsignedLEShort();
			streamOffsets = new int[objectCount];
			int offset = 2;
			for(int index = 0; index < objectCount; index++)
			{
				streamOffsets[index] = offset;
				offset += indexStream.getUnsignedLEShort();
			}

			cache = new GameObjectDefinition[20];
			for(int c = 0; c < 20; c++)
			{
				cache[c] = new GameObjectDefinition();
			}
		}

		public static void nullLoader()
		{
			modelCache = null;
			animatedModelCache = null;
			streamOffsets = null;
			cache = null;
			stream = null;
		}

		public bool unknownAttribute;

		/// <summary>
		/// Ambient lighting modifier.
		/// </summary>
		public byte ambient { get; private set; }

		public int translateX { get; private set; }

		public String name;

		public int scaleZ { get; private set; }

		/// <summary>
		/// Diffuse lighting modifier.
		/// </summary>
		public byte diffuse { get; private set; }

		public int sizeX;

		public int translateY { get; private set; }

		public int icon;
		public int[] originalModelColors { get; private set; }
		public int scaleX { get; private set; }
		public int configIds;
		public bool rotated { get; private set; }
		public static bool lowMemory;
		private static Default317Buffer stream;
		public int id;
		private static int[] streamOffsets;
		public bool walkable;
		public int mapScene;
		public int[] childIds;
		public int _solid { get; private set; }
		public int sizeY;
		public bool adjustToTerrain;
		public bool wall;
		private bool unwalkableSolid;
		public bool solid;
		public int face;
		public bool delayShading { get; private set; }
		private static int cacheIndex;
		public int scaleY { get; private set; }
		public int[] modelIds { get; private set; }
		public int varBitId;
		public int offsetAmplifier;
		public int[] modelTypes { get; private set; }
		public byte[] description;
		public bool hasActions;
		public bool castsShadow;
		public static Cache animatedModelCache = new Cache(30);
		public int animationId;
		private static GameObjectDefinition[] cache;
		public int translateZ { get; private set; }
		public int[] modifiedModelColors { get; private set; }
		public static Cache modelCache = new Cache(500);
		public String[] actions;

		public static int DefinitionCount => streamOffsets != null ? streamOffsets.Length : 0;

		private GameObjectDefinition()
		{
			id = -1;
		}

		public GameObjectDefinition getChildDefinition(IInterfaceSettingsProvider interfaceSettingsProvider)
		{
			int child = -1;
			if(varBitId != -1)
			{
				VarBit varBit = VarBit.values[varBitId];
				int configId = varBit.configId;
				int lsb = varBit.leastSignificantBit;
				int msb = varBit.mostSignificantBit;
				int bit = ConstantData.GetBitfieldMaxValue(msb - lsb);
				child = interfaceSettingsProvider.GetInterfaceSettings(configId) >> lsb & bit;
			}
			else if(configIds != -1)
			{
				child = interfaceSettingsProvider.GetInterfaceSettings(configIds);
			}

			if(child < 0 || child >= childIds.Length || childIds[child] == -1)
			{
				return null;
			}
			else
			{
				return getDefinition(childIds[child]);
			}
		}

		private void loadDefinition(Default317Buffer stream)
		{
			int _actions = -1;
			do
			{
				int opcode;
				do
				{
					opcode = stream.getUnsignedByte();
					if(opcode == 0)
						goto label0;

					if(opcode == 1)
					{
						int modelCount = stream.getUnsignedByte();
						if(modelCount > 0)
						{
							if(modelIds == null || lowMemory)
							{
								modelTypes = new int[modelCount];
								modelIds = new int[modelCount];
								for(int m = 0; m < modelCount; m++)
								{
									modelIds[m] = stream.getUnsignedLEShort();
									modelTypes[m] = stream.getUnsignedByte();
								}

							}
							else
							{
								stream.position += modelCount * 3;
							}
						}
					}
					else if(opcode == 2)
					{
						name = stream.getString();
					}
					else if(opcode == 3)
					{
						description = stream.readBytes();
					}
					else if(opcode == 5)
					{
						int modelCount = stream.getUnsignedByte();
						if(modelCount > 0)
						{
							if(modelIds == null || lowMemory)
							{
								modelTypes = null;
								modelIds = new int[modelCount];
								for(int m = 0; m < modelCount; m++)
								{
									modelIds[m] = stream.getUnsignedLEShort();
								}

							}
							else
							{
								stream.position += modelCount * 2;
							}
						}
					}
					else if(opcode == 14)
					{
						sizeX = stream.getUnsignedByte();
					}
					else if(opcode == 15)
					{
						sizeY = stream.getUnsignedByte();
					}
					else if(opcode == 17)
					{
						solid = false;
					}
					else if(opcode == 18)
					{
						walkable = false;
					}
					else if(opcode == 19)
					{
						_actions = stream.getUnsignedByte();
						if(_actions == 1)
						{
							hasActions = true;
						}
					}
					else if(opcode == 21)
					{
						adjustToTerrain = true;
					}
					else if(opcode == 22)
					{
						delayShading = true;
					}
					else if(opcode == 23)
					{
						wall = true;
					}
					else if(opcode == 24)
					{
						animationId = stream.getUnsignedLEShort();
						if(animationId == 65535)
						{
							animationId = -1;
						}
					}
					else if(opcode == 28)
					{
						offsetAmplifier = stream.getUnsignedByte();
					}
					else if(opcode == 29)
					{
						ambient = stream.get();
					}
					else if(opcode == 39)
					{
						diffuse = stream.get();
					}
					else if(opcode >= 30 && opcode < 39)
					{
						if(actions == null)
						{
							actions = new String[5];
						}

						actions[opcode - 30] = stream.getString();
						if(actions[opcode - 30].Equals("hidden", StringComparison.InvariantCultureIgnoreCase))
						{
							actions[opcode - 30] = null;
						}
					}
					else if(opcode == 40)
					{
						int colourCount = stream.getUnsignedByte();
						modifiedModelColors = new int[colourCount];
						originalModelColors = new int[colourCount];
						for(int c = 0; c < colourCount; c++)
						{
							modifiedModelColors[c] = stream.getUnsignedLEShort();
							originalModelColors[c] = stream.getUnsignedLEShort();
						}

					}
					else if(opcode == 60)
					{
						icon = stream.getUnsignedLEShort();
					}
					else if(opcode == 62)
					{
						rotated = true;
					}
					else if(opcode == 64)
					{
						castsShadow = false;
					}
					else if(opcode == 65)
					{
						scaleX = stream.getUnsignedLEShort();
					}
					else if(opcode == 66)
					{
						scaleY = stream.getUnsignedLEShort();
					}
					else if(opcode == 67)
					{
						scaleZ = stream.getUnsignedLEShort();
					}
					else if(opcode == 68)
					{
						mapScene = stream.getUnsignedLEShort();
					}
					else if(opcode == 69)
					{
						face = stream.getUnsignedByte();
					}
					else if(opcode == 70)
					{
						translateX = stream.getShort();
					}
					else if(opcode == 71)
					{
						translateY = stream.getShort();
					}
					else if(opcode == 72)
					{
						translateZ = stream.getShort();
					}
					else if(opcode == 73)
					{
						unknownAttribute = true;
					}
					else if(opcode == 74)
					{
						unwalkableSolid = true;
					}
					else
					{
						if(opcode != 75)
						{
							continue;
						}

						_solid = stream.getUnsignedByte();
					}

					continue;
				} while(opcode != 77);

				varBitId = stream.getUnsignedLEShort();
				if(varBitId == 65535)
				{
					varBitId = -1;
				}

				configIds = stream.getUnsignedLEShort();
				if(configIds == 65535)
				{
					configIds = -1;
				}

				int childCount = stream.getUnsignedByte();
				childIds = new int[childCount + 1];
				for(int c = 0; c <= childCount; c++)
				{
					childIds[c] = stream.getUnsignedLEShort();
					if(childIds[c] == 65535)
					{
						childIds[c] = -1;
					}
				}

			} while(true);

			label0:
			if(_actions == -1)
			{
				hasActions = modelIds != null && (modelTypes == null || modelTypes[0] == 10);
				if(actions != null)
				{
					hasActions = true;
				}
			}

			if(unwalkableSolid)
			{
				solid = false;
				walkable = false;
			}

			if(_solid == -1)
			{
				_solid = solid ? 1 : 0;
			}
		}

		private void setDefaults()
		{
			modelIds = null;
			modelTypes = null;
			name = null;
			description = null;
			modifiedModelColors = null;
			originalModelColors = null;
			sizeX = 1;
			sizeY = 1;
			solid = true;
			walkable = true;
			hasActions = false;
			adjustToTerrain = false;
			delayShading = false;
			wall = false;
			animationId = -1;
			offsetAmplifier = 16;
			ambient = 0;
			diffuse = 0;
			actions = null;
			icon = -1;
			mapScene = -1;
			rotated = false;
			castsShadow = true;
			scaleX = 128;
			scaleY = 128;
			scaleZ = 128;
			face = 0;
			translateX = 0;
			translateY = 0;
			translateZ = 0;
			unknownAttribute = false;
			unwalkableSolid = false;
			_solid = -1;
			varBitId = -1;
			configIds = -1;
			childIds = null;
		}
	}
}