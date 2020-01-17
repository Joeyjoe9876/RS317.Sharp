using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public static class GameObjectDefinitionExtensions
	{
		//This is copy-pasted from the old GameObjectDefinition.
		private static Model[] models = new Model[4];

		public static Model getAnimatedModel(this GameObjectDefinition definition, int type, int animationId, int face)
		{
			Model subModel = null;
			long hash;
			if(definition.modelTypes == null)
			{
				if(type != 10)
				{
					return null;
				}

				hash = (definition.id << 6) + face + ((long)(animationId + 1) << 32);
				Model cachedModel = (Model)GameObjectDefinition.animatedModelCache.get(hash);
				if(cachedModel != null)
				{
					return cachedModel;
				}

				if(definition.modelIds == null)
				{
					return null;
				}

				bool mirror = definition.rotated ^ (face > 3);
				int modelCount = definition.modelIds.Length;
				for(int m = 0; m < modelCount; m++)
				{
					int subModelId = definition.modelIds[m];
					if(mirror)
					{
						subModelId += 0x10000;
					}

					subModel = (Model)GameObjectDefinition.modelCache.get(subModelId);
					if(subModel == null)
					{
						subModel = Model.getModel(subModelId & 0xffff);
						if(subModel == null)
						{
							return null;
						}

						if(mirror)
						{
							subModel.mirror();
						}

						GameObjectDefinition.modelCache.put(subModel, subModelId);
					}

					if(modelCount > 1)
					{
						models[m] = subModel;
					}
				}

				if(modelCount > 1)
				{
					subModel = new Model(modelCount, models);
				}
			}
			else
			{
				int modelType = -1;
				for(int t = 0; t < definition.modelTypes.Length; t++)
				{
					if(definition.modelTypes[t] != type)
					{
						continue;
					}

					modelType = t;
					break;
				}

				if(modelType == -1)
				{
					return null;
				}

				hash = (definition.id << 6) + (modelType << 3) + face + ((long)(animationId + 1) << 32);
				Model model = (Model)GameObjectDefinition.animatedModelCache.get(hash);
				if(model != null)
				{
					return model;
				}

				int modelId = definition.modelIds[modelType];
				bool mirror = definition.rotated ^ (face > 3);
				if(mirror)
				{
					modelId += 0x10000;
				}

				subModel = (Model)GameObjectDefinition.modelCache.get(modelId);
				if(subModel == null)
				{
					subModel = Model.getModel(modelId & 0xffff);
					if(subModel == null)
					{
						return null;
					}

					if(mirror)
					{
						subModel.mirror();
					}

					GameObjectDefinition.modelCache.put(subModel, modelId);
				}
			}

			bool scale;
			scale = definition.scaleX != 128 || definition.scaleY != 128 || definition.scaleZ != 128;
			bool translate;
			translate = definition.translateX != 0 || definition.translateY != 0 || definition.translateZ != 0;
			Model animatedModel = new Model(definition.modifiedModelColors == null, Animation.isNullFrame(animationId),
				face == 0 && animationId == -1 && !scale && !translate, subModel);
			if(animationId != -1)
			{
				animatedModel.createBones();
				animatedModel.applyTransformation(animationId);
				animatedModel.triangleSkin = null;
				animatedModel.vertexSkin = null;
			}

			while(face-- > 0)
			{
				animatedModel.rotate90Degrees();
			}

			if(definition.modifiedModelColors != null)
			{
				for(int c = 0; c < definition.modifiedModelColors.Length; c++)
				{
					animatedModel.recolour(definition.modifiedModelColors[c], definition.originalModelColors[c]);
				}

			}

			if(scale)
			{
				animatedModel.scaleT(definition.scaleX, definition.scaleZ, definition.scaleY);
			}

			if(translate)
			{
				animatedModel.translate(definition.translateX, definition.translateY, definition.translateZ);
			}

			animatedModel.applyLighting(64 + definition.ambient, 768 + definition.diffuse * 5, -50, -10, -50, !definition.delayShading);
			if(definition._solid == 1)
			{
				animatedModel.anInt1654 = animatedModel.modelHeight;
			}

			GameObjectDefinition.animatedModelCache.put(animatedModel, hash);
			return animatedModel;
		}

		public static Model getModelAt(this GameObjectDefinition definition, int i, int j, int k, int l, int i1, int j1, int k1)
		{
			Model model = definition.getAnimatedModel(i, k1, j);
			if(model == null)
			{
				return null;
			}

			if(definition.adjustToTerrain || definition.delayShading)
			{
				model = new Model(definition.adjustToTerrain, definition.delayShading, model);
			}

			if(definition.adjustToTerrain)
			{
				int l1 = (k + l + i1 + j1) / 4;
				for(int v = 0; v < model.vertexCount; v++)
				{
					int x = model.verticesX[v];
					int z = model.verticesZ[v];
					int l2 = k + ((l - k) * (x + 64)) / 128;
					int i3 = j1 + ((i1 - j1) * (x + 64)) / 128;
					int j3 = l2 + ((i3 - l2) * (z + 64)) / 128;
					model.verticesY[v] += j3 - l1;
				}

				model.normalise();
			}

			return model;
		}

		public static bool modelCached(this GameObjectDefinition definition)
		{
			if(definition.modelIds == null)
			{
				return true;
			}

			bool cached = true;
			for(int m = 0; m < definition.modelIds.Length; m++)
			{
				cached &= Model.isCached(definition.modelIds[m] & 0xffff);
			}

			return cached;
		}

		public static bool modelTypeCached(this GameObjectDefinition definition, int modelType)
		{
			if(definition.modelTypes == null)
			{
				if(definition.modelIds == null)
				{
					return true;
				}

				if(modelType != 10)
				{
					return true;
				}

				bool cached = true;
				for(int id = 0; id < definition.modelIds.Length; id++)
				{
					cached &= Model.isCached(definition.modelIds[id] & 0xffff);
				}

				return cached;
			}

			for(int type = 0; type < definition.modelTypes.Length; type++)
			{
				if(definition.modelTypes[type] == modelType)
				{
					return Model.isCached(definition.modelIds[type] & 0xffff);
				}
			}

			return true;
		}

		public static void passivelyRequestModels(this GameObjectDefinition definition, OnDemandFetcher requester)
		{
			if(definition.modelIds == null)
			{
				return;
			}

			for(int modelId = 0; modelId < definition.modelIds.Length; modelId++)
			{
				requester.passiveRequest(definition.modelIds[modelId] & 0xffff, 0);
			}
		}
	}
}
