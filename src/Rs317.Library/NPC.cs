
namespace Rs317.Sharp
{
	public sealed class NPC : Entity
	{
		public EntityDefinition npcDefinition;

		public NPC()
		{
		}

		private Model getChildModel()
		{
			if(base.animation >= 0 && base.animationDelay == 0)
			{
				int frameId2 = AnimationSequence.animations[base.animation].primaryFrames[base.currentAnimationFrame];
				int frameId1 = -1;
				if(base.queuedAnimationId >= 0 && base.queuedAnimationId != base.standAnimationId)
					frameId1 = AnimationSequence.animations[base.queuedAnimationId].primaryFrames[base.queuedAnimationFrame];
				return npcDefinition.getChildModel(frameId1, frameId2,
					AnimationSequence.animations[base.animation].flowControl);
			}
			else
			{
				int frameId2 = -1;
				if(base.queuedAnimationId >= 0)
					frameId2 = AnimationSequence.animations[base.queuedAnimationId].primaryFrames[base.queuedAnimationFrame];
				return npcDefinition.getChildModel(-1, frameId2, null);
			}
		}

		public override Model getRotatedModel()
		{
			if(npcDefinition == null)
				return null;
			Model rotatedModel = getChildModel();
			if(rotatedModel == null)
				return null;
			base.height = rotatedModel.modelHeight;
			if(base.graphicId != -1 && base.currentAnimationId != -1)
			{
				SpotAnimation spotAnimation = SpotAnimation.cache[base.graphicId];
				Model animationModel = spotAnimation.getModel();
				if(animationModel != null)
				{
					int frameId = spotAnimation.sequences.primaryFrames[base.currentAnimationId];
					Model animatedModel = new Model(true, Animation.isNullFrame(frameId), false, animationModel);
					animatedModel.translate(0, -base.graphicHeight, 0);
					animatedModel.createBones();
					animatedModel.applyTransformation(frameId);
					animatedModel.triangleSkin = null;
					animatedModel.vertexSkin = null;
					if(spotAnimation.scaleXY != 128 || spotAnimation.scaleZ != 128)
						animatedModel.scaleT(spotAnimation.scaleXY, spotAnimation.scaleXY, spotAnimation.scaleZ);
					animatedModel.applyLighting(64 + spotAnimation.modelLightFalloff, 850 + spotAnimation.modelLightAmbient,
						-30, -50, -30, true);
					Model[] models = { rotatedModel, animatedModel };
					rotatedModel = new Model(models);
				}
			}

			if(npcDefinition.boundaryDimension == 1)
				rotatedModel.singleTile = true;
			return rotatedModel;
		}

		public override bool isVisible()
		{
			return npcDefinition != null;
		}
	}
}
