using System;
using AvatarWrapper;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class Player : Actor
{
	public enum State
	{
		WAITING_AT_START,
		READYSTEADY,
		RACING,
		FINISHED
	}

	public enum EngineState
	{
		NONE,
		IDLE,
		ACCELERATING,
		DECELERATING,
		BRAKING
	}

	private const float BIKE_CRASH_TOL_SQ = 9f;

	private const float BODY_HIT_TOL_SQ = 9f;

	private const float IMPACT_SFX_DELAY = 500f;

	private const float LAND_VELY = -2f;

	public const float Z_DISTANCE = 0f;

	public const float GROUND_HEIGHT = 0f;

	public const float LEFT_EDGE = 141f;

	public const float RIGHT_EDGE = 1122f;

	public const float TOP_EDGE = 73f;

	public const int PLAYER_HEALTH = 100;

	public const int PLAYER_RECOVER = 6;

	public const float GEAR_REVS_UP = 2000f;

	public const float GEAR_REVS_DOWN = 4000f;

	public const float GEAR_REVS_INC = 25f;

	public const float GEAR_REVS_DEC = 35f;

	private const float AVATAR_SCALE = 2f;

	private const float GEAR_VEL_1 = 10f;

	private const float GEAR_VEL_2 = 20f;

	private const float GEAR_VEL_3 = 30f;

	private const float GEAR_VEL_4 = 40f;

	private const float GEAR_VEL_5 = 50f;

	private const float GEAR_BASEREVS_1 = 1000f;

	private const float GEAR_BASEREVS_2 = 1000f;

	private const float GEAR_BASEREVS_3 = 2000f;

	private const float GEAR_BASEREVS_4 = 2000f;

	private const float GEAR_BASEREVS_5 = 3000f;

	public const float PLAYER_GRAVITY = 0.5f;

	public const float JUMP_VEL = -0.4f;

	public const float JUMP_GAIN = 41f;

	public const float MAX_VEL_X = 0.5f;

	public const float MAX_VEL_Y = 0.5f;

	public const float PLAYER_ACCEL = 1f;

	public const float VEL_UPGRADE = 1.25f;

	private const float STUNT_START_TIME = 500f;

	private const int STUNT_SCORE = 100;

	private const float STUNT_MIN_VELX = 0.25f;

	public PrismaticJoint m_RearPrismatic;

	public PrismaticJoint m_FrontPrismatic;

	public RevoluteJoint m_RearRevolute;

	public RevoluteJoint m_FrontRevolute;

	public Body m_ChasisBody;

	public Body m_RearSusBody;

	public Body m_FrontForkBody;

	public Body m_BackWheelBody;

	public Body m_FrontWheelBody;

	public Fixture m_ChasisFixture;

	public Fixture m_ChasisFixtureRear;

	public Fixture m_ChasisFixtureFront;

	public Fixture m_BackWheelFixture;

	public Fixture m_FrontWheelFixture;

	public Matrix m_BackWheelOrgTransform;

	public Matrix m_FrontWheelOrgTransform;

	public Matrix m_SwingArmOrgTransform;

	public Matrix m_FrontForkOrgTransform;

	public Matrix m_BikeBoneOrgTransform;

	public Ragdoll m_Ragdoll;

	public bool m_bRequestCrash;

	public float m_BackWheelOnFloorTime;

	public bool m_bBackWheelOnGround;

	public bool m_BackWheelLanded;

	public float m_FrontWheelOnFloorTime;

	public bool m_bFrontWheelOnGround;

	public bool m_FrontWheelLanded;

	public float m_ImpactSoundTime;

	public float m_BikeImpactSoundTime;

	public float m_BackWheelImpactSoundTime;

	public float m_FrontWheelImpactSoundTime;

	public Vector3 AVATAR_OFFSET = new Vector3(0f, 0f, 0f);

	public Vector2 HAMMER_OFFSET3D = new Vector2(1f, 0.5f);

	public Vector2 BAT_OFFSET3D = new Vector2(1.5f, -0.4f);

	public Vector2 PINBALL_OFFSET3D = new Vector2(-0.5f, -1f);

	public Vector2 PINBALL_BALL_OFFSET3D = new Vector2(1.2f, -1f);

	public float PICKUP_HAMMER_DURATION = 20f;

	public float PICKUP_CLOUD_DURATION = 20f;

	public float PICKUP_BAT_DURATION = 50f;

	public bool m_bOnGround;

	public bool m_bPrevOnGround;

	public bool m_bDebounceJump;

	public bool m_bDoCollision = true;

	public GamePadState m_OldGamepadState;

	public GamePadState m_GamepadState;

	public PlayerIndex m_PadId;

	public Avatar m_Avatar;

	public int m_AssignAvatarCount = 500;

	public float m_PropRot;

	public float m_Roll;

	public float m_Height;

	public float m_LAX;

	public float m_LAY;

	public int m_SmokeTrailId = -1;

	public int m_WheelTrailId = -1;

	public int m_SparkTrailId = -1;

	public float m_fNextFireTime;

	public bool m_bDying;

	public int m_Health = 100;

	public int m_Recover;

	public int m_MaxRecover = 6;

	public int m_Score;

	public int m_ScoreTally;

	public int m_LevelScore;

	public float m_AirTime;

	public int m_AirScore;

	public float m_MaxAirTime;

	public int m_MaxAirScore;

	public float m_AirDisplayScoreTime;

	public int m_AirDisplayScore;

	public int m_SpinRevolutions;

	public int m_SpinDisplayRevolutions;

	public int m_SpinScore;

	public float m_SpinDisplayScoreTime;

	public bool m_BackFlip;

	public float m_EndoTime;

	public int m_EndoScore;

	public float m_MaxEndoTime;

	public int m_MaxEndoScore;

	public float m_EndoDisplayScoreTime;

	public int m_EndoDisplayScore;

	public float m_WheelieTime;

	public int m_WheelieScore;

	public float m_MaxWheelieTime;

	public int m_MaxWheelieScore;

	public float m_WheelieDisplayScoreTime;

	public int m_WheelieDisplayScore;

	public bool m_bCanBeOnGround = true;

	public float m_TaxiingTime;

	public float m_WaitingAtTowerTime;

	public int m_RumbleFrames;

	public SoundEffectInstance m_EngineSound;

	public SoundEffectInstance m_EngineIdleSound;

	public SoundEffectInstance m_EngineAccelerateSound;

	public SoundEffectInstance m_EngineDecelerateSound;

	public SoundEffectInstance m_EngineBrakeSound;

	public State m_State;

	public int m_DeathSmokeId = -1;

	public int m_DeathSparksId = -1;

	public int m_Rank;

	public float m_InvulnerableTime;

	public int m_ModelId = -1;

	public int m_ScrapeCounter;

	public bool m_bShowAButton;

	public int m_ShowAButtonCounter;

	public bool m_bShowBButton;

	public int m_ShowBButtonCounter;

	public SoundEffectInstance m_DiveSound;

	public int m_GestureIndex;

	public float m_NextGesture;

	public bool m_bSignedIn;

	public float m_Revs;

	public float m_PrevRevs;

	public float m_Gear;

	public float m_RevScale;

	public float m_ReadySteadyTime;

	public EngineState m_EngineState = EngineState.IDLE;

	public EngineState m_PrevEngineState;

	public int m_FlagsCollected;

	public float m_SkidTime;

	public void SetupPhysics(bool bReset, Vector2 startPos)
	{
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Expected O, but got Unknown
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Expected O, but got Unknown
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Expected O, but got Unknown
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Expected O, but got Unknown
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Expected O, but got Unknown
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_077b: Expected O, but got Unknown
		//IL_077b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Expected O, but got Unknown
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		ModelBone val = m_Model.Bones["Bike"];
		ModelBone val2 = m_Model.Bones["BackWheel"];
		ModelBone val3 = m_Model.Bones["FrontWheel"];
		ModelBone val4 = m_Model.Bones["SwingArm"];
		ModelBone val5 = m_Model.Bones["FrontFork"];
		if (!bReset)
		{
			m_BikeBoneOrgTransform = val.Transform;
			((Matrix)(ref m_BikeBoneOrgTransform)).Translation = Vector3.Zero;
			m_BackWheelOrgTransform = val2.Transform;
			((Matrix)(ref m_BackWheelOrgTransform)).Translation = Vector3.Zero;
			m_FrontWheelOrgTransform = val3.Transform;
			m_SwingArmOrgTransform = val4.Transform;
			((Matrix)(ref m_SwingArmOrgTransform)).Translation = Vector3.Zero;
			m_FrontForkOrgTransform = val5.Transform;
			((Matrix)(ref m_FrontForkOrgTransform)).Translation = Vector3.Zero;
			ContactManager contactManager = Program.m_App.m_World.ContactManager;
			contactManager.BeginContact = (BeginContactDelegate)Delegate.Combine((Delegate)(object)contactManager.BeginContact, (Delegate)new BeginContactDelegate(BeginContact));
		}
		Vector2 val6 = default(Vector2);
		((Vector2)(ref val6))._002Ector(0f, 0.11f);
		float num = 3f;
		float num2 = 1.5f;
		float num3 = 0.5f;
		float num4 = 0.1f;
		Vector2 val7 = startPos + new Vector2(-1f, 1f);
		m_ChasisBody = BodyFactory.CreateBody(Program.m_App.m_World);
		m_ChasisBody.BodyType = (BodyType)2;
		m_ChasisBody.Position = val7;
		PolygonShape val8 = new PolygonShape();
		val8.SetAsBox(num2 / 2f, num3 / 2f);
		PolygonShape val9 = new PolygonShape();
		val9.SetAsBox(0.4f, 0.15f, new Vector2(-0.75f, val6.Y + num4), -(float)Math.PI / 3f);
		PolygonShape val10 = new PolygonShape();
		val10.SetAsBox(0.4f, 0.15f, new Vector2(0.75f, val6.Y + num4), -(float)Math.PI / 3f);
		m_ChasisFixture = m_ChasisBody.CreateFixture((Shape)(object)val8, num);
		m_ChasisFixtureRear = m_ChasisBody.CreateFixture((Shape)(object)val9, num);
		m_ChasisFixtureFront = m_ChasisBody.CreateFixture((Shape)(object)val10, num);
		Fixture chasisFixture = m_ChasisFixture;
		Fixture chasisFixtureRear = m_ChasisFixtureRear;
		Fixture chasisFixtureFront = m_ChasisFixtureFront;
		CollisionCategory val11 = (CollisionCategory)1;
		chasisFixtureFront.CollisionCategories = (CollisionCategory)1;
		CollisionCategory collisionCategories = (chasisFixtureRear.CollisionCategories = val11);
		chasisFixture.CollisionCategories = collisionCategories;
		Fixture chasisFixture2 = m_ChasisFixture;
		Fixture chasisFixtureRear2 = m_ChasisFixtureRear;
		Fixture chasisFixtureFront2 = m_ChasisFixtureFront;
		CollisionCategory val13 = (CollisionCategory)1073741824;
		chasisFixtureFront2.CollidesWith = (CollisionCategory)1073741824;
		CollisionCategory collidesWith = (chasisFixtureRear2.CollidesWith = val13);
		chasisFixture2.CollidesWith = collidesWith;
		float num5 = num;
		m_RearSusBody = BodyFactory.CreateBody(Program.m_App.m_World);
		m_RearSusBody.BodyType = (BodyType)2;
		m_RearSusBody.Position = val7 + new Vector2(-0.98f, -0.2f);
		float num6 = (float)Math.PI / 2f;
		PolygonShape val15 = new PolygonShape();
		val15.SetAsBox(0.4f, 0.1f, Vector2.Zero, num6);
		Fixture val16 = m_RearSusBody.CreateFixture((Shape)(object)val15, num5);
		val16.CollisionCategories = (CollisionCategory)1;
		val16.CollidesWith = (CollisionCategory)1073741824;
		m_RearPrismatic = JointFactory.CreatePrismaticJoint(m_ChasisBody, m_RearSusBody, Vector2.Zero, new Vector2((float)Math.Cos(num6), (float)Math.Sin(num6)));
		m_RearPrismatic.LowerLimit = -0.5f;
		m_RearPrismatic.UpperLimit = 0.5f;
		m_RearPrismatic.LimitEnabled = true;
		m_RearPrismatic.MotorEnabled = true;
		m_RearPrismatic.MotorSpeed = 0f;
		m_RearPrismatic.MaxMotorForce = 10000f;
		((Joint)m_RearPrismatic).CollideConnected = false;
		Program.m_App.m_World.AddJoint((Joint)(object)m_RearPrismatic);
		float num7 = num;
		m_FrontForkBody = BodyFactory.CreateBody(Program.m_App.m_World);
		m_FrontForkBody.BodyType = (BodyType)2;
		m_FrontForkBody.Position = val7 + new Vector2(0.98f, -0.2f);
		float num8 = 1.0971975f;
		PolygonShape val17 = new PolygonShape();
		val17.SetAsBox(0.4f, 0.1f, Vector2.Zero, 0f - num8);
		Fixture val18 = m_FrontForkBody.CreateFixture((Shape)(object)val17, num7);
		val18.CollisionCategories = (CollisionCategory)1;
		val18.CollidesWith = (CollisionCategory)1073741824;
		m_FrontPrismatic = JointFactory.CreatePrismaticJoint(m_ChasisBody, m_FrontForkBody, Vector2.Zero, new Vector2((float)(0.0 - Math.Cos(num8)), (float)Math.Sin(num8)));
		m_FrontPrismatic.LowerLimit = -0.5f;
		m_FrontPrismatic.UpperLimit = 0.5f;
		m_FrontPrismatic.LimitEnabled = true;
		m_FrontPrismatic.MotorEnabled = true;
		m_FrontPrismatic.MotorSpeed = 0f;
		m_FrontPrismatic.MaxMotorForce = 10000f;
		((Joint)m_FrontPrismatic).CollideConnected = false;
		Program.m_App.m_World.AddJoint((Joint)(object)m_FrontPrismatic);
		float num9 = 0.1f;
		float num10 = 0.53f;
		Vector2 position = default(Vector2);
		((Vector2)(ref position))._002Ector(val7.X - 1.45f + 0.25f, val7.Y - 0.64f + num4);
		m_BackWheelBody = BodyFactory.CreateBody(Program.m_App.m_World);
		m_BackWheelBody.BodyType = (BodyType)2;
		m_BackWheelBody.Position = position;
		CircleShape val19 = new CircleShape(num10);
		m_BackWheelFixture = m_BackWheelBody.CreateFixture((Shape)(object)val19, num9);
		m_BackWheelFixture.CollisionCategories = (CollisionCategory)1;
		m_BackWheelFixture.CollidesWith = (CollisionCategory)1073741824;
		m_BackWheelFixture.Friction = 200f;
		Fixture backWheelFixture = m_BackWheelFixture;
		backWheelFixture.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)backWheelFixture.OnCollision, (Delegate)new CollisionEventHandler(BackWheelOnCollision));
		Fixture backWheelFixture2 = m_BackWheelFixture;
		backWheelFixture2.OnSeparation = (SeparationEventHandler)Delegate.Combine((Delegate)(object)backWheelFixture2.OnSeparation, (Delegate)new SeparationEventHandler(BackWheelOnSeparation));
		m_RearRevolute = JointFactory.CreateRevoluteJoint(m_RearSusBody, m_BackWheelBody, Vector2.Zero);
		m_RearRevolute.MotorEnabled = true;
		m_RearRevolute.MaxMotorTorque = 1f;
		((Joint)m_RearRevolute).CollideConnected = false;
		Program.m_App.m_World.AddJoint((Joint)(object)m_RearRevolute);
		float num11 = 0.1f;
		float num12 = 0.53f;
		Vector2 position2 = default(Vector2);
		((Vector2)(ref position2))._002Ector(val7.X + 0.9f + 0.255f, val7.Y - 0.64f + num4);
		m_FrontWheelBody = BodyFactory.CreateBody(Program.m_App.m_World);
		m_FrontWheelBody.BodyType = (BodyType)2;
		m_FrontWheelBody.Position = position2;
		CircleShape val20 = new CircleShape(num12);
		m_FrontWheelFixture = m_FrontWheelBody.CreateFixture((Shape)(object)val20, num11);
		m_FrontWheelFixture.Friction = 15f;
		m_FrontWheelFixture.CollisionCategories = (CollisionCategory)1;
		m_FrontWheelFixture.CollidesWith = (CollisionCategory)1073741824;
		Fixture frontWheelFixture = m_FrontWheelFixture;
		frontWheelFixture.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)frontWheelFixture.OnCollision, (Delegate)new CollisionEventHandler(FrontWheelOnCollision));
		Fixture frontWheelFixture2 = m_FrontWheelFixture;
		frontWheelFixture2.OnSeparation = (SeparationEventHandler)Delegate.Combine((Delegate)(object)frontWheelFixture2.OnSeparation, (Delegate)new SeparationEventHandler(FrontWheelOnSeparation));
		m_FrontRevolute = JointFactory.CreateRevoluteJoint(m_FrontForkBody, m_FrontWheelBody, Vector2.Zero);
		m_FrontRevolute.MotorEnabled = true;
		m_FrontRevolute.MaxMotorTorque = 0f;
		((Joint)m_FrontRevolute).CollideConnected = false;
		Program.m_App.m_World.AddJoint((Joint)(object)m_FrontRevolute);
		val.Transform = m_BikeBoneOrgTransform;
		val4.Transform = m_SwingArmOrgTransform;
		m_Ragdoll = new Ragdoll(Program.m_App.m_World, new Vector2(val7.X - 0.4f, val7.Y + 1.8f));
		m_bRequestCrash = false;
		if (m_Avatar != null)
		{
			m_Avatar.StartAnimation(Program.m_App.m_IdleGripAnimation, LoopAnimation: false);
			m_Avatar.IsLoopingAnimation = true;
		}
	}

	public void UpdatePlayerFromPhysics()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		m_Position.X = m_ChasisBody.Position.X;
		m_Position.Y = m_ChasisBody.Position.Y;
		m_Rotation.X = m_ChasisBody.Rotation;
		UpdateChasis();
		UpdateBackWheel();
		UpdateFontWheel();
		UpdateFontFork();
		UpdateSwingArm();
		m_Model.CopyAbsoluteBoneTransformsTo(m_Transforms);
	}

	public void UpdateChasis()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		ModelBone val = m_Model.Bones["Bike"];
		Vector3 val2 = default(Vector3);
		val2.X = m_ChasisBody.Position.X - 0.14f;
		val2.Y = m_ChasisBody.Position.Y + 0.25f;
		val2.Z = 0f;
		val2.X -= m_Position.X;
		val2.Y -= m_Position.Y;
		Matrix val3 = Matrix.CreateTranslation(0f, val2.Y, 0f - val2.X);
		((Matrix)(ref m_BikeBoneOrgTransform)).Translation = Vector3.Zero;
		val.Transform = m_BikeBoneOrgTransform * val3;
	}

	public void UpdateBackWheel()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		ModelBone val = m_Model.Bones["BackWheel"];
		Matrix val2 = Matrix.CreateRotationX(m_BackWheelBody.Rotation);
		Vector3 zero = Vector3.Zero;
		zero.X = m_BackWheelBody.Position.X;
		if (m_BackWheelBody.Position.Y < 0f)
		{
			m_ChasisBody.SetTransform(new Vector2(m_ChasisBody.Position.X, m_ChasisBody.Position.Y + 0.5f), m_ChasisBody.Rotation);
		}
		zero.Y = m_BackWheelBody.Position.Y;
		zero.Z = 0f;
		Matrix val3 = Matrix.CreateRotationZ(0f - m_Rotation.X);
		zero.X -= m_Position.X;
		zero.Y -= m_Position.Y;
		zero = Vector3.Transform(zero, val3);
		Matrix val4 = Matrix.CreateTranslation(0f, zero.Y, 0f - zero.X);
		val.Transform = val2 * m_BackWheelOrgTransform * val4;
	}

	public void UpdateFontWheel()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		ModelBone val = m_Model.Bones["FrontWheel"];
		Matrix val2 = Matrix.CreateRotationX(m_FrontWheelBody.Rotation);
		Vector3 zero = Vector3.Zero;
		zero.X = m_FrontWheelBody.Position.X;
		zero.Y = m_FrontWheelBody.Position.Y;
		zero.Z = 0f;
		Matrix val3 = Matrix.CreateRotationZ(0f - m_Rotation.X);
		zero.X -= m_Position.X;
		zero.Y -= m_Position.Y;
		zero = Vector3.Transform(zero, val3);
		Matrix val4 = Matrix.CreateTranslation(0f, zero.Y, 0f - zero.X);
		((Matrix)(ref m_FrontWheelOrgTransform)).Translation = Vector3.Zero;
		val.Transform = val2 * m_FrontWheelOrgTransform * val4;
	}

	public void UpdateFontFork()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		ModelBone val = m_Model.Bones["FrontWheel"];
		Matrix transform = val.Transform;
		Matrix val2 = Matrix.CreateTranslation(((Matrix)(ref transform)).Translation);
		ModelBone val3 = m_Model.Bones["FrontFork"];
		val3.Transform = m_FrontForkOrgTransform * val2;
	}

	public void UpdateSwingArm()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		ModelBone val = m_Model.Bones["BackWheel"];
		Matrix transform = val.Transform;
		Matrix val2 = Matrix.CreateTranslation(((Matrix)(ref transform)).Translation);
		ModelBone val3 = m_Model.Bones["SwingArm"];
		val3.Transform = m_SwingArmOrgTransform * val2;
		Vector2 val4 = default(Vector2);
		((Vector2)(ref val4))._002Ector(1f, 0f);
		Vector2 zero = Vector2.Zero;
		Matrix transform2 = val3.Transform;
		zero.X = ((Matrix)(ref transform2)).Translation.Z;
		Matrix transform3 = val3.Transform;
		zero.Y = ((Matrix)(ref transform3)).Translation.Y;
		Vector2 val5 = zero;
		((Vector2)(ref val5)).Normalize();
		float num = Vector2.Dot(val4, val5);
		float num2 = (float)Math.Acos(num);
		Matrix val6 = Matrix.CreateRotationX(num2 * 1.5f - 0.6f);
		val3.Transform = val6 * m_SwingArmOrgTransform * val2;
	}

	public void UpdateAvatarRagdoll()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		if (m_Avatar != null && m_Avatar.currentXNACustomAnimation.BoneTransforms != null)
		{
			_ = Matrix.Identity;
			Matrix val = Matrix.Identity;
			_ = Matrix.Identity;
			float num = 0f;
			if (m_Ragdoll.m_bOnBike)
			{
				num = m_Rotation.X;
			}
			val = Matrix.CreateRotationX(0f - m_Ragdoll._head.Body.Rotation + num);
			ref Matrix reference = ref m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[14];
			reference = val * GetInvParents(14);
			val = Matrix.CreateRotationX(0f - m_Ragdoll._body[0].Body.Rotation + num);
			((Matrix)(ref val)).Translation = ((Matrix)(ref val)).Translation + new Vector3(0f, -0.33f, -0.3f);
			m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[0] = val;
			val = Matrix.CreateRotationX(0f - m_Ragdoll._upperLeftArm[0].Body.Rotation + num);
			ref Matrix reference2 = ref m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[12];
			reference2 = val * GetInvParents(12);
			val = Matrix.CreateRotationX(0f - m_Ragdoll._upperRightArm[0].Body.Rotation + num);
			ref Matrix reference3 = ref m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[16];
			reference3 = val * GetInvParents(16);
			float num2 = 0.5f;
			val = Matrix.CreateRotationX(0f - m_Ragdoll._lowerLeftArm[0].Body.Rotation + num);
			Matrix val2 = Matrix.CreateRotationY(num2);
			Matrix val3 = m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[20];
			((Matrix)(ref val3)).Translation = Vector3.Zero;
			ref Matrix reference4 = ref m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[25];
			reference4 = val3 * val * val2 * GetInvParents(25);
			val = Matrix.CreateRotationX(0f - m_Ragdoll._lowerRightArm[0].Body.Rotation + num);
			val2 = Matrix.CreateRotationY(0f - num2);
			val3 = m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[22];
			((Matrix)(ref val3)).Translation = Vector3.Zero;
			ref Matrix reference5 = ref m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[28];
			reference5 = val3 * val * val2 * GetInvParents(28);
			val = Matrix.CreateRotationX(0f - m_Ragdoll._upperLeftLeg[0].Body.Rotation + num);
			ref Matrix reference6 = ref m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[2];
			reference6 = val * GetInvParents(2);
			val = Matrix.CreateRotationX(0f - m_Ragdoll._upperRightLeg[0].Body.Rotation + num);
			ref Matrix reference7 = ref m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[3];
			reference7 = val * GetInvParents(3);
			val = Matrix.CreateRotationX(0f - m_Ragdoll._lowerLeftLeg[0].Body.Rotation + num);
			ref Matrix reference8 = ref m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[6];
			reference8 = val * GetInvParents(6);
			val = Matrix.CreateRotationX(0f - m_Ragdoll._lowerRightLeg[0].Body.Rotation + num);
			ref Matrix reference9 = ref m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[8];
			reference9 = val * GetInvParents(8);
		}
	}

	private Matrix GetInvParents(int startBone)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		int num = Avatar.parentBones[startBone];
		Matrix val = Matrix.Identity;
		Matrix val2 = Matrix.Identity;
		while (num != -1)
		{
			val2 = Matrix.Invert(m_Avatar.currentXNACustomAnimation.avatarBoneTransforms[num]);
			((Matrix)(ref val2)).Translation = Vector3.Zero;
			val *= val2;
			num = Avatar.parentBones[num];
		}
		return val;
	}

	protected virtual void BeginContact(Contact contact)
	{
		if (contact.FixtureA == m_Ragdoll._head && !IsRagdollBody(contact.FixtureB))
		{
			RequestCrash();
		}
		if (contact.FixtureB == m_Ragdoll._head && !IsRagdollBody(contact.FixtureA))
		{
			RequestCrash();
		}
		CheckBikeImpactSounds(contact);
		if (!m_Ragdoll.m_bOnBike)
		{
			CheckRagdollImpactSounds(contact);
		}
	}

	public bool IsRagdollBody(Fixture f)
	{
		if (f == m_Ragdoll._body[0] || f == m_Ragdoll._body[1] || f == m_Ragdoll._body[2] || f == m_Ragdoll._body[3] || f == m_Ragdoll._upperLeftArm[0] || f == m_Ragdoll._upperLeftArm[1] || f == m_Ragdoll._upperLeftArm[2] || f == m_Ragdoll._lowerLeftArm[0] || f == m_Ragdoll._lowerLeftArm[1] || f == m_Ragdoll._lowerLeftArm[2] || f == m_Ragdoll._upperRightArm[0] || f == m_Ragdoll._upperRightArm[1] || f == m_Ragdoll._upperRightArm[2] || f == m_Ragdoll._lowerRightArm[0] || f == m_Ragdoll._lowerRightArm[1] || f == m_Ragdoll._lowerRightArm[2])
		{
			return true;
		}
		return false;
	}

	public void RequestCrash()
	{
		if (m_Ragdoll.m_bOnBike)
		{
			m_bRequestCrash = true;
		}
	}

	public void Crash()
	{
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		m_bRequestCrash = false;
		m_Ragdoll.m_bOnBike = false;
		if (m_Ragdoll.HandlebarLeftRevolute != null)
		{
			Program.m_App.m_World.RemoveJoint((Joint)(object)m_Ragdoll.HandlebarLeftRevolute);
			m_Ragdoll.HandlebarLeftRevolute = null;
		}
		if (m_Ragdoll.HandlebarRightRevolute != null)
		{
			Program.m_App.m_World.RemoveJoint((Joint)(object)m_Ragdoll.HandlebarRightRevolute);
			m_Ragdoll.HandlebarRightRevolute = null;
		}
		if (m_Ragdoll.PedalLeftRevolute != null)
		{
			Program.m_App.m_World.RemoveJoint((Joint)(object)m_Ragdoll.PedalLeftRevolute);
			m_Ragdoll.PedalLeftRevolute = null;
		}
		if (m_Ragdoll.PedalRightRevolute != null)
		{
			Program.m_App.m_World.RemoveJoint((Joint)(object)m_Ragdoll.PedalRightRevolute);
			m_Ragdoll.PedalRightRevolute = null;
		}
		if (m_Ragdoll.TorsoRevolute != null)
		{
			Program.m_App.m_World.RemoveJoint((Joint)(object)m_Ragdoll.TorsoRevolute);
			m_Ragdoll.TorsoRevolute = null;
		}
		Fixture head = m_Ragdoll._head;
		head.CollidesWith = (CollisionCategory)(head.CollidesWith | 1);
		for (int i = 0; i < m_Ragdoll._body.Count; i++)
		{
			Fixture obj = m_Ragdoll._body[i];
			obj.CollidesWith = (CollisionCategory)(obj.CollidesWith | 1);
		}
		for (int j = 0; j < m_Ragdoll._lowerLeftLeg.Count; j++)
		{
			Fixture obj2 = m_Ragdoll._lowerLeftLeg[j];
			obj2.CollidesWith = (CollisionCategory)(obj2.CollidesWith | 0x40000000);
		}
		for (int k = 0; k < m_Ragdoll._lowerRightLeg.Count; k++)
		{
			Fixture obj3 = m_Ragdoll._lowerRightLeg[k];
			obj3.CollidesWith = (CollisionCategory)(obj3.CollidesWith | 0x40000000);
		}
		if (m_Avatar != null)
		{
			m_Avatar.StartAnimation(Program.m_App.m_IdleAnimation, LoopAnimation: false);
			m_Avatar.IsLoopingAnimation = true;
		}
		m_Revs = 0f;
		if (m_BikeImpactSoundTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			Vector2 linearVelocity = m_ChasisBody.LinearVelocity;
			float num = ((Vector2)(ref linearVelocity)).LengthSquared() / 36f;
			num = MathHelper.Clamp(num, 1f, 3f);
			Program.m_SoundManager.Play(PickCrashSound(), num / 3f);
		}
		Vector2 linearVelocity2 = m_ChasisBody.LinearVelocity;
		float num2 = ((Vector2)(ref linearVelocity2)).LengthSquared() / 36f;
		num2 = MathHelper.Clamp(num2, 1f, 4f);
		Program.m_CameraManager3D.Shake(500f, num2 * 0.4f);
		Program.m_App.m_RumbleFrames += 10 * (int)num2;
	}

	public void CheckBikeImpactSounds(Contact contact)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
		if (m_BikeImpactSoundTime > num)
		{
			return;
		}
		if ((contact.FixtureA == m_ChasisFixture || contact.FixtureA == m_ChasisFixtureRear || contact.FixtureA == m_ChasisFixtureFront) && !IsRagdollBody(contact.FixtureB))
		{
			Vector2 linearVelocity = m_ChasisFixture.Body.LinearVelocity;
			if (((Vector2)(ref linearVelocity)).LengthSquared() > 9f)
			{
				Vector2 linearVelocity2 = m_ChasisBody.LinearVelocity;
				float num2 = ((Vector2)(ref linearVelocity2)).LengthSquared() / 36f;
				num2 = MathHelper.Clamp(num2, 1f, 3f);
				Program.m_SoundManager.Play3D(PickCrashSound(), m_ChasisBody.Position, num2 / 3f);
				m_BikeImpactSoundTime = num + 500f;
				Vector2 linearVelocity3 = m_ChasisFixture.Body.LinearVelocity;
				float num3 = ((Vector2)(ref linearVelocity3)).LengthSquared() / 9f;
				num3 = MathHelper.Clamp(num3, 1f, 2f);
				Program.m_CameraManager3D.Shake(250f, num3 * 0.1f);
				Program.m_App.m_RumbleFrames += (int)num3;
			}
		}
		if ((contact.FixtureB == m_ChasisFixture || contact.FixtureB == m_ChasisFixtureRear || contact.FixtureB == m_ChasisFixtureFront) && !IsRagdollBody(contact.FixtureA))
		{
			Vector2 linearVelocity4 = m_ChasisFixture.Body.LinearVelocity;
			if (((Vector2)(ref linearVelocity4)).LengthSquared() > 9f)
			{
				Vector2 linearVelocity5 = m_ChasisBody.LinearVelocity;
				float num4 = ((Vector2)(ref linearVelocity5)).LengthSquared() / 36f;
				num4 = MathHelper.Clamp(num4, 1f, 3f);
				Program.m_SoundManager.Play3D(PickCrashSound(), m_ChasisBody.Position, num4 / 3f);
				m_BikeImpactSoundTime = num + 500f;
				Vector2 linearVelocity6 = m_ChasisFixture.Body.LinearVelocity;
				float num5 = ((Vector2)(ref linearVelocity6)).LengthSquared() / 9f;
				num5 = MathHelper.Clamp(num5, 1f, 2f);
				Program.m_CameraManager3D.Shake(250f, num5 * 0.1f);
				Program.m_App.m_RumbleFrames += (int)num5;
			}
		}
	}

	public void CheckRagdollImpactSounds(Contact contact)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
		if (m_ImpactSoundTime > num)
		{
			return;
		}
		if (contact.FixtureA == m_Ragdoll._body[0] && !IsRagdollBody(contact.FixtureB))
		{
			Vector2 linearVelocity = contact.FixtureA.Body.LinearVelocity;
			if (((Vector2)(ref linearVelocity)).LengthSquared() > 9f)
			{
				Program.m_SoundManager.Play(10 + Program.m_App.NonRandomRandom(5));
				m_ImpactSoundTime = num + 500f;
				Program.m_App.m_RumbleFrames += 3;
			}
		}
		if (contact.FixtureB == m_Ragdoll._body[0] && !IsRagdollBody(contact.FixtureA))
		{
			Vector2 linearVelocity2 = contact.FixtureB.Body.LinearVelocity;
			if (((Vector2)(ref linearVelocity2)).LengthSquared() > 9f)
			{
				Program.m_SoundManager.Play(10 + Program.m_App.NonRandomRandom(5));
				m_ImpactSoundTime = num + 500f;
				Program.m_App.m_RumbleFrames += 3;
			}
		}
	}

	protected virtual bool BackWheelOnCollision(Fixture FixtureB, Fixture FixtureA, Contact contact)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		if (!m_bBackWheelOnGround && m_ChasisBody.LinearVelocity.Y < -2f)
		{
			m_BackWheelLanded = true;
			float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
			if (m_BackWheelImpactSoundTime < num)
			{
				Program.m_SoundManager.Play(22);
				m_BackWheelImpactSoundTime = num + 250f;
				float num2 = m_ChasisBody.LinearVelocity.Y / -2f;
				num2 = MathHelper.Clamp(num2, 1f, 2f);
				Program.m_App.m_RumbleFrames += (int)num2;
			}
		}
		else
		{
			m_BackWheelLanded = false;
		}
		m_BackWheelOnFloorTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
		m_bBackWheelOnGround = true;
		return true;
	}

	protected virtual void BackWheelOnSeparation(Fixture FixtureB, Fixture FixtureA)
	{
		m_bBackWheelOnGround = false;
	}

	protected virtual bool FrontWheelOnCollision(Fixture FixtureB, Fixture FixtureA, Contact contact)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		if (!m_bFrontWheelOnGround && m_ChasisBody.LinearVelocity.Y < -2f)
		{
			m_FrontWheelLanded = true;
			float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
			if (m_FrontWheelImpactSoundTime < num)
			{
				Program.m_SoundManager.Play(23);
				m_FrontWheelImpactSoundTime = num + 250f;
				float num2 = m_ChasisBody.LinearVelocity.Y / -2f;
				num2 = MathHelper.Clamp(num2, 1f, 2f);
				Program.m_App.m_RumbleFrames += (int)num2;
			}
		}
		else
		{
			m_FrontWheelLanded = false;
		}
		m_FrontWheelOnFloorTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
		m_bFrontWheelOnGround = true;
		return true;
	}

	protected virtual void FrontWheelOnSeparation(Fixture FixtureB, Fixture FixtureA)
	{
		m_bFrontWheelOnGround = false;
	}

	public void UpdateAvatarShadow()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		Program.m_App.m_AvShad_Head.m_Position = m_Ragdoll._head.Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_Head.m_Rotation.Z = m_Ragdoll._head.Body.Rotation;
		Program.m_App.m_AvShad_Body.m_Position = m_Ragdoll.Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_Body.m_Rotation.Z = m_Ragdoll.Body.Rotation;
		Program.m_App.m_AvShad_UpperArmR.m_Position = m_Ragdoll._upperRightArm[0].Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_UpperArmR.m_Rotation.Z = m_Ragdoll._upperRightArm[0].Body.Rotation;
		Program.m_App.m_AvShad_UpperArmR.m_ZDistance = -0.2f;
		Program.m_App.m_AvShad_UpperArmL.m_Position = m_Ragdoll._upperLeftArm[0].Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_UpperArmL.m_Rotation.Z = m_Ragdoll._upperLeftArm[0].Body.Rotation;
		Program.m_App.m_AvShad_UpperArmL.m_ZDistance = 0.2f;
		Program.m_App.m_AvShad_LowerArmR.m_Position = m_Ragdoll._lowerRightArm[0].Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_LowerArmR.m_Rotation.Z = m_Ragdoll._lowerRightArm[0].Body.Rotation;
		Program.m_App.m_AvShad_LowerArmR.m_ZDistance = -0.2f;
		Program.m_App.m_AvShad_LowerArmL.m_Position = m_Ragdoll._lowerLeftArm[0].Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_LowerArmL.m_Rotation.Z = m_Ragdoll._lowerLeftArm[0].Body.Rotation;
		Program.m_App.m_AvShad_LowerArmL.m_ZDistance = 0.2f;
		Program.m_App.m_AvShad_UpperLegR.m_Position = m_Ragdoll._upperRightLeg[0].Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_UpperLegR.m_Rotation.Z = m_Ragdoll._upperRightLeg[0].Body.Rotation;
		Program.m_App.m_AvShad_UpperLegR.m_ZDistance = -0.2f;
		Program.m_App.m_AvShad_UpperLegL.m_Position = m_Ragdoll._upperLeftLeg[0].Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_UpperLegL.m_Rotation.Z = m_Ragdoll._upperLeftLeg[0].Body.Rotation;
		Program.m_App.m_AvShad_UpperLegL.m_ZDistance = 0.2f;
		Program.m_App.m_AvShad_LowerLegR.m_Position = m_Ragdoll._lowerRightLeg[0].Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_LowerLegR.m_Rotation.Z = m_Ragdoll._lowerRightLeg[0].Body.Rotation;
		Program.m_App.m_AvShad_LowerLegR.m_ZDistance = -0.2f;
		Program.m_App.m_AvShad_LowerLegL.m_Position = m_Ragdoll._lowerLeftLeg[0].Body.Position + new Vector2(-0.5f, 0f);
		Program.m_App.m_AvShad_LowerLegL.m_Rotation.Z = m_Ragdoll._lowerLeftLeg[0].Body.Rotation;
		Program.m_App.m_AvShad_LowerLegL.m_ZDistance = 0.2f;
	}

	public int PickCrashSound()
	{
		float num = (float)Program.m_App.m_Rand.NextDouble();
		if (num < 0.2f)
		{
			return 15;
		}
		if (num < 0.4f)
		{
			return 16;
		}
		if (num < 0.6f)
		{
			return 17;
		}
		if (num < 0.7f)
		{
			return 18;
		}
		return 19;
	}

	public Player()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		m_Velocity = Vector2.Zero;
		m_bOnGround = false;
		m_Rotation.Y = -(float)Math.PI / 2f;
	}

	public void Delete()
	{
		m_Id = -1;
		m_Avatar = null;
		m_AssignAvatarCount = 100;
		if (m_SmokeTrailId != -1)
		{
			Program.m_ParticleManager.Delete(m_SmokeTrailId);
			m_SmokeTrailId = -1;
		}
		if (m_SparkTrailId != -1)
		{
			Program.m_ParticleManager.Delete(m_SparkTrailId);
			m_SparkTrailId = -1;
		}
		if (m_WheelTrailId != -1)
		{
			Program.m_ParticleManager.Delete(m_WheelTrailId);
			m_WheelTrailId = -1;
		}
		if (m_DeathSmokeId != -1)
		{
			Program.m_ParticleManager.Delete(m_DeathSmokeId);
			m_DeathSmokeId = -1;
		}
		if (m_DeathSparksId != -1)
		{
			Program.m_ParticleManager.Delete(m_DeathSparksId);
			m_DeathSparksId = -1;
		}
		DeletePhysics();
	}

	public void Render()
	{
	}

	public void RenderAvatar()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		if (m_Avatar != null)
		{
			Vector3 zero = Vector3.Zero;
			if (m_Ragdoll.m_bOnBike)
			{
				zero.X = m_Position.X;
				zero.Y = m_Position.Y;
				m_Avatar.Rotation = m_Rotation;
			}
			else
			{
				zero.X = m_Ragdoll.Body.Position.X + 0.5f;
				zero.Y = m_Ragdoll.Body.Position.Y - 0.9f;
				m_Avatar.Rotation = new Vector3(0f, m_Rotation.Y, 0f);
			}
			zero.Z = 0f;
			m_Avatar.Position = zero;
			m_Avatar.Scale = 2f;
			m_Avatar.View = Program.m_CameraManager3D.m_CameraViewMatrix;
			m_Avatar.Projection = Program.m_CameraManager3D.m_CameraProjectionMatrix;
			m_Avatar.Draw();
		}
	}

	public void Update()
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		if (m_FlashModel > 0)
		{
			m_FlashModel--;
		}
		UpdateShake();
		if (Program.m_App.m_Paused)
		{
			UpdateControls(0f);
			return;
		}
		UpdateControls(0.01667f);
		UpdateSuspension();
		UpdateGears();
		UpdatePlayerFromPhysics();
		if (m_bRequestCrash)
		{
			Crash();
		}
		CheckFinishLine();
		UpdateState();
		ClampPositions();
		m_Height = m_Position.Y - 0f;
		if (m_Height < 0f)
		{
			m_Height = 0f;
		}
		UpdateScore();
		CheckDead();
		m_PrevPosition = m_Position;
		m_PrevRotation = m_Rotation;
		m_PrevVelocity = m_Velocity;
		UpdateEngineSound();
		if (Gamer.SignedInGamers[m_PadId] != null && !m_bSignedIn)
		{
			m_Avatar = null;
			m_AssignAvatarCount = 100;
		}
		if (m_Avatar != null)
		{
			UpdateAvatarRagdoll();
			UpdateAvatarShadow();
		}
		else
		{
			if (m_AssignAvatarCount > 0)
			{
				m_AssignAvatarCount--;
			}
			if (Gamer.SignedInGamers[m_PadId] != null && Gamer.SignedInGamers[m_PadId].Avatar.IsValid)
			{
				m_Avatar = new Avatar(Gamer.SignedInGamers[m_PadId].Avatar);
				m_Avatar.StartAnimation(Program.m_App.m_IdleGripAnimation, LoopAnimation: false);
			}
			else if (m_AssignAvatarCount == 0)
			{
				m_Avatar = new Avatar(AvatarDescription.CreateRandom());
				m_Avatar.StartAnimation(Program.m_App.m_IdleGripAnimation, LoopAnimation: false);
			}
		}
		CheckDyingPrompt();
		CalcBounds2D(bUseAll: false, bUsePitch: false);
		if (Gamer.SignedInGamers[m_PadId] != null)
		{
			m_bSignedIn = true;
		}
	}

	private void CheckFinishLine()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (m_State != State.FINISHED)
		{
			Item item = Program.m_ItemManager.FindObjectByType(14);
			if (item != null && item.m_Position.X < m_Ragdoll.Body.Position.X && m_State == State.RACING)
			{
				m_State = State.FINISHED;
				Program.m_App.m_TextScale = 3f;
				AddToOnlineScoreBoard();
				Program.m_LoadSaveManager.SaveCheckpoint(Vector2.Zero, bShowMessage: false);
				m_Score += m_ScoreTally;
				m_Score += m_WheelieScore;
				m_Score += m_EndoScore;
				m_Score += m_AirScore;
				m_Score += m_SpinScore;
				Program.m_SoundManager.Play(28);
			}
		}
	}

	private void AddToOnlineScoreBoard()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_App.m_Level <= 10 && !Guide.IsTrialMode && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
		{
			TopScoreEntry entry = new TopScoreEntry(((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag, (int)Program.m_App.m_TrackTime);
			Program.m_App.mScores.addEntry(Program.m_App.m_Level - 1, entry, Program.m_App.mSyncManager);
		}
	}

	private void CheckDyingPrompt()
	{
		if (m_bDying && Program.m_App.m_LevelReached > 1)
		{
			m_ShowAButtonCounter++;
			if (m_ShowAButtonCounter >= 7)
			{
				m_bShowAButton = !m_bShowAButton;
				m_ShowAButtonCounter = 0;
			}
		}
	}

	private void ClampPositions()
	{
		m_bOnGround = false;
		if (m_Position.Y <= 0f)
		{
			m_Position.Y = 0f;
			m_bOnGround = true;
		}
	}

	private void UpdateWheelTrail()
	{
	}

	public void UpdateState()
	{
		float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
		switch (m_State)
		{
		case State.WAITING_AT_START:
			if (!Program.m_App.Fading() && m_ReadySteadyTime - num < 1500f)
			{
				Program.m_SoundManager.Play(20);
				m_State = State.READYSTEADY;
				Program.m_App.m_TextScale = 3f;
			}
			break;
		case State.READYSTEADY:
			if (m_ReadySteadyTime < num)
			{
				Program.m_SoundManager.Play(21);
				m_State = State.RACING;
				Program.m_App.m_TextScale = 3f;
			}
			break;
		case State.RACING:
		case State.FINISHED:
			break;
		}
	}

	public static Vector3 ScreenToWorld(Vector3 screenPos)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Viewport viewport = ((Game)Program.m_App).GraphicsDevice.Viewport;
		return ((Viewport)(ref viewport)).Unproject(screenPos, Program.m_CameraManager3D.m_CameraProjectionMatrix, Program.m_CameraManager3D.m_CameraViewMatrix, Matrix.Identity);
	}

	private void UpdateControls(float t)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Invalid comparison between Unknown and I4
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Invalid comparison between Unknown and I4
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Invalid comparison between Unknown and I4
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Invalid comparison between Unknown and I4
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Invalid comparison between Unknown and I4
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Invalid comparison between Unknown and I4
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		m_GamepadState = GamePad.GetState(m_PadId);
		if (m_bDebounceJump)
		{
			GamePadButtons buttons = ((GamePadState)(ref m_GamepadState)).Buttons;
			if ((int)((GamePadButtons)(ref buttons)).A == 0)
			{
				m_bDebounceJump = false;
			}
		}
		if (!Program.m_App.m_Paused && Program.m_App.m_State == App.STATE.INGAME && (Debounce((Buttons)16) || (Program.m_App.Debounce((Keys)32) && !Program.m_App.Fading())))
		{
			Program.m_SoundManager.Play(2);
			Program.m_App.StartPause(m_Id, m_PadId);
		}
		CheckJukeboxControls();
		if (m_bDying)
		{
			m_OldGamepadState = m_GamepadState;
			return;
		}
		if (Debounce((Buttons)32768))
		{
			if ((m_State == State.RACING && !m_Ragdoll.m_bOnBike) || m_State == State.FINISHED)
			{
				Program.m_LoadSaveManager.LoadCheckpoint(ref m_Position);
				ResetBike(bForce: false, restartRace: true, Vector2.Zero);
			}
			else
			{
				Program.m_SoundManager.Play(31);
			}
		}
		if (Debounce((Buttons)16384) && Program.m_LoadSaveManager.m_TrackTime != 0f && m_State == State.RACING && !m_Ragdoll.m_bOnBike)
		{
			Program.m_LoadSaveManager.LoadCheckpoint(ref m_Position);
			Program.m_App.m_TrackTime += 1000f;
			ResetBike(bForce: false, restartRace: false, m_Position);
		}
		if (m_Ragdoll.m_bOnBike && Debounce((Buttons)8192))
		{
			Body body = m_Ragdoll.Body;
			body.LinearVelocity += new Vector2(0f, 100f);
			Crash();
		}
		if (m_State == State.FINISHED && Debounce((Buttons)4096))
		{
			Program.m_App.m_NextState = App.STATE.LEVEL_END;
			Program.m_App.StartFade(up: false);
			Program.m_SoundManager.Play(2);
		}
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref m_GamepadState)).ThumbSticks;
		m_LAX = DeadZone(((GamePadThumbSticks)(ref thumbSticks)).Left.X);
		GamePadThumbSticks thumbSticks2 = ((GamePadState)(ref m_GamepadState)).ThumbSticks;
		m_LAY = DeadZone(((GamePadThumbSticks)(ref thumbSticks2)).Left.Y);
		if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)37))
		{
			m_LAX = -1f;
		}
		if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)39))
		{
			m_LAX = 1f;
		}
		if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)40))
		{
			m_LAY = -1f;
		}
		if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)38))
		{
			m_LAY = 1f;
		}
		GamePadDPad dPad = ((GamePadState)(ref m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad)).Left == 1)
		{
			m_LAX = -1f;
		}
		GamePadDPad dPad2 = ((GamePadState)(ref m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad2)).Right == 1)
		{
			m_LAX = 1f;
		}
		GamePadDPad dPad3 = ((GamePadState)(ref m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad3)).Down == 1)
		{
			m_LAY = -1f;
		}
		GamePadDPad dPad4 = ((GamePadState)(ref m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad4)).Up == 1)
		{
			m_LAY = 1f;
		}
		if (Math.Abs(m_LAX) + Math.Abs(m_LAY) > 1.4f)
		{
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(m_LAX, m_LAY);
			((Vector2)(ref val)).Normalize();
			m_LAX = val.X;
			m_LAY = val.Y;
		}
		if (Math.Abs(m_LAX) > 0f)
		{
			m_Ragdoll._head.Body.ApplyTorque((0f - m_LAX) * 50f);
		}
		else if (!m_bFrontWheelOnGround && !m_bBackWheelOnGround)
		{
			if (m_Rotation.X < 0f)
			{
				m_Ragdoll._head.Body.ApplyTorque(-10f);
			}
			else
			{
				m_Ragdoll._head.Body.ApplyTorque(10f);
			}
		}
		if (m_State != 0 && m_State != State.READYSTEADY)
		{
			GamePadTriggers triggers = ((GamePadState)(ref m_GamepadState)).Triggers;
			float num = ((GamePadTriggers)(ref triggers)).Right;
			GamePadTriggers triggers2 = ((GamePadState)(ref m_GamepadState)).Triggers;
			float num2 = ((GamePadTriggers)(ref triggers2)).Left;
			GamePadButtons buttons2 = ((GamePadState)(ref m_GamepadState)).Buttons;
			if ((int)((GamePadButtons)(ref buttons2)).A == 1)
			{
				num = 1f;
			}
			GamePadButtons buttons3 = ((GamePadState)(ref m_GamepadState)).Buttons;
			if ((int)((GamePadButtons)(ref buttons3)).X == 1)
			{
				num2 = 1f;
			}
			m_PrevRevs = m_Revs;
			m_EngineState = EngineState.IDLE;
			if (num > 0f && m_Ragdoll.m_bOnBike)
			{
				m_RearRevolute.MotorSpeed = -37.5f * num;
				m_RearRevolute.MaxMotorTorque = 45f * Math.Abs(num);
				m_FrontRevolute.MaxMotorTorque = 0f;
				m_Revs += m_RevScale * 0.1f;
				m_EngineState = EngineState.ACCELERATING;
			}
			else if (num2 > 0f && m_Ragdoll.m_bOnBike)
			{
				if (m_ChasisBody.LinearVelocity.X > 0f)
				{
					m_RearRevolute.MaxMotorTorque = MathHelper.Lerp(m_RearRevolute.MaxMotorTorque, 500f * num2, 0.2f);
					m_RearRevolute.MotorSpeed = MathHelper.Lerp(m_RearRevolute.MotorSpeed, 0f, 0.2f);
					m_FrontRevolute.MaxMotorTorque = MathHelper.Lerp(m_FrontRevolute.MaxMotorTorque, 30f * num2, 0.2f);
					m_FrontRevolute.MotorSpeed = MathHelper.Lerp(m_FrontRevolute.MotorSpeed, 0f, 0.8f);
				}
				else
				{
					m_RearRevolute.MotorSpeed = 12f * num2;
					m_RearRevolute.MaxMotorTorque = 30f * Math.Abs(num2);
					m_FrontRevolute.MaxMotorTorque = 0f;
				}
				m_Revs -= 35f;
				m_EngineState = EngineState.BRAKING;
			}
			else
			{
				m_RearRevolute.MaxMotorTorque = 1f;
				m_FrontRevolute.MaxMotorTorque = 0f;
				m_Revs -= 35f;
			}
			float num3 = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
			if (num2 == 1f && m_BackWheelBody.AngularVelocity < 0.1f && Math.Abs(m_ChasisBody.LinearVelocity.X) > 15f && m_SkidTime < num3 && m_bBackWheelOnGround && m_bFrontWheelOnGround)
			{
				if (Program.m_App.m_Rand.NextDouble() > 0.5)
				{
					Program.m_SoundManager.Play(26);
				}
				else
				{
					Program.m_SoundManager.Play(27);
				}
				m_SkidTime += 3000f;
			}
			else if (num2 < 1f)
			{
				Program.m_SoundManager.Stop(26);
				Program.m_SoundManager.Stop(27);
			}
		}
		m_OldGamepadState = m_GamepadState;
	}

	public void CheckJukeboxControls()
	{
		if (Debounce((Buttons)256))
		{
			Program.m_App.PlayPrevSong();
		}
		if (Debounce((Buttons)512))
		{
			Program.m_App.PlayNextSong();
		}
	}

	public void DeletePhysics()
	{
		if (m_ChasisBody != null)
		{
			Program.m_App.m_World.RemoveBody(m_ChasisBody);
			Program.m_App.m_World.RemoveBody(m_RearSusBody);
			Program.m_App.m_World.RemoveBody(m_FrontForkBody);
			Program.m_App.m_World.RemoveBody(m_BackWheelBody);
			Program.m_App.m_World.RemoveBody(m_FrontWheelBody);
			Program.m_App.m_World.RemoveBody(m_Ragdoll._head.Body);
			Program.m_App.m_World.RemoveBody(m_Ragdoll.Body);
			Program.m_App.m_World.RemoveBody(m_Ragdoll._upperLeftArm[0].Body);
			Program.m_App.m_World.RemoveBody(m_Ragdoll._lowerLeftArm[0].Body);
			Program.m_App.m_World.RemoveBody(m_Ragdoll._upperLeftLeg[0].Body);
			Program.m_App.m_World.RemoveBody(m_Ragdoll._lowerLeftLeg[0].Body);
			Program.m_App.m_World.RemoveBody(m_Ragdoll._upperRightArm[0].Body);
			Program.m_App.m_World.RemoveBody(m_Ragdoll._lowerRightArm[0].Body);
			Program.m_App.m_World.RemoveBody(m_Ragdoll._upperRightLeg[0].Body);
			Program.m_App.m_World.RemoveBody(m_Ragdoll._lowerRightLeg[0].Body);
			m_ChasisBody = null;
		}
	}

	public void ResetBike(bool bForce, bool restartRace, Vector2 startpos)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		if (m_State != State.FINISHED && m_Ragdoll.m_bOnBike && !bForce)
		{
			return;
		}
		GC.Collect();
		DeletePhysics();
		m_Rotation.X = 0f;
		m_Rotation.Z = 0f;
		SetupPhysics(bReset: true, startpos);
		UpdatePlayerFromPhysics();
		CalcBounds2D(bUseAll: false, bUsePitch: false);
		Program.m_App.m_BackgroundManager.Reset();
		m_Gear = 0f;
		m_Revs = 0f;
		m_EngineState = EngineState.IDLE;
		m_AirTime = 0f;
		m_AirScore = 0;
		m_WheelieTime = 0f;
		m_WheelieScore = 0;
		m_EndoTime = 0f;
		m_EndoScore = 0;
		if (restartRace)
		{
			Program.m_App.m_TrackTime = 0f;
			m_Score = 0;
			m_ScoreTally = 0;
			m_State = State.WAITING_AT_START;
			m_ReadySteadyTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 2000f;
			if (Program.m_App.m_Tip < App.TEXTID.TIP14)
			{
				Program.m_App.m_Tip++;
				Program.m_App.m_TipDisplayTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 5000f;
			}
			Program.m_LoadSaveManager.SaveCheckpoint(Vector2.Zero, bShowMessage: false);
		}
		Program.m_App.ReLoadLevel();
	}

	private void UpdateSuspension()
	{
		float num = 0f;
		num = 30f + Math.Abs(500f * m_RearPrismatic.JointTranslation);
		if (num > 250f)
		{
			num = 250f;
		}
		m_RearPrismatic.MaxMotorForce = num;
		m_RearPrismatic.MotorSpeed = (m_RearPrismatic.MotorSpeed - 2f * m_RearPrismatic.JointTranslation) * 0.4f;
		num = 30f + Math.Abs(500f * m_FrontPrismatic.JointTranslation);
		if (num > 250f)
		{
			num = 250f;
		}
		m_FrontPrismatic.MaxMotorForce = num;
		m_FrontPrismatic.MotorSpeed = (m_FrontPrismatic.MotorSpeed - 2f * m_FrontPrismatic.JointTranslation) * 0.4f;
	}

	private void UpdateGears()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		m_RevScale = 500f;
		if (m_PrevRevs <= m_Revs)
		{
			if (m_Revs > 6000f)
			{
				m_Revs = 6000f;
			}
			if (m_Gear != 6f)
			{
				if (m_Gear == 0f && m_ChasisBody.LinearVelocity.X > 10f)
				{
					m_Gear += 1f;
					m_Revs *= 0.333f;
				}
				else if (m_Gear == 1f && m_ChasisBody.LinearVelocity.X > 20f)
				{
					m_Gear += 1f;
					m_Revs *= 0.333f;
				}
				else if (m_Gear == 2f && m_ChasisBody.LinearVelocity.X > 30f)
				{
					m_Gear += 1f;
					m_Revs *= 0.333f;
				}
				else if (m_Gear == 3f && m_ChasisBody.LinearVelocity.X > 40f)
				{
					m_Gear += 1f;
					m_Revs *= 0.333f;
				}
				else if (m_Gear == 4f && m_ChasisBody.LinearVelocity.X > 50f)
				{
					m_Gear += 1f;
					m_Revs *= 0.333f;
				}
			}
		}
		else
		{
			if (m_Gear > 0f && m_Revs < 1000f)
			{
				m_Gear -= 1f;
				m_Revs = 4000f;
			}
			if (m_Revs < 0f)
			{
				m_Revs = 0f;
			}
			if (m_Gear < 0f)
			{
				m_Revs = 0f;
			}
		}
	}

	public void UpdateEngineSound()
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		if (!m_Ragdoll.m_bOnBike)
		{
			m_EngineState = EngineState.IDLE;
		}
		switch (m_EngineState)
		{
		case EngineState.IDLE:
		{
			if (m_PrevEngineState != EngineState.IDLE && m_EngineState == EngineState.IDLE)
			{
				StopAllEngineSounds();
				if (m_EngineIdleSound == null)
				{
					m_EngineIdleSound = Program.m_SoundManager.PlayLooped(8);
				}
				else
				{
					m_EngineIdleSound.Resume();
				}
			}
			else
			{
				m_EngineIdleSound.Resume();
			}
			_ = m_EngineIdleSound.Pitch;
			Vector2 linearVelocity2 = m_ChasisBody.LinearVelocity;
			float value2 = ((Vector2)(ref linearVelocity2)).Length();
			float num2 = Math.Abs(value2) / 20f;
			num2 -= 0.9f;
			num2 = MathHelper.Clamp(num2, -0.9f, 0f);
			m_EngineIdleSound.Pitch = num2;
			break;
		}
		case EngineState.ACCELERATING:
		{
			if (m_PrevEngineState != EngineState.ACCELERATING && m_EngineState == EngineState.ACCELERATING)
			{
				StopAllEngineSounds();
				if (m_EngineSound == null)
				{
					m_EngineSound = Program.m_SoundManager.PlayLooped(9);
				}
				else
				{
					m_EngineSound.Resume();
				}
			}
			else
			{
				m_EngineSound.Resume();
			}
			_ = m_EngineSound.Pitch;
			Vector2 linearVelocity3 = m_ChasisBody.LinearVelocity;
			float value3 = ((Vector2)(ref linearVelocity3)).Length();
			float num3 = Math.Abs(value3) / 20f;
			num3 -= 0.9f;
			num3 = MathHelper.Clamp(num3, -0.9f, 0f);
			m_EngineSound.Pitch = num3;
			break;
		}
		case EngineState.BRAKING:
		{
			if (m_PrevEngineState != EngineState.BRAKING && m_EngineState == EngineState.BRAKING)
			{
				StopAllEngineSounds();
				if (m_EngineBrakeSound == null)
				{
					m_EngineBrakeSound = Program.m_SoundManager.PlayLooped(8);
				}
				else
				{
					m_EngineBrakeSound.Resume();
				}
			}
			else
			{
				m_EngineBrakeSound.Resume();
			}
			_ = m_EngineBrakeSound.Pitch;
			Vector2 linearVelocity = m_ChasisBody.LinearVelocity;
			float value = ((Vector2)(ref linearVelocity)).Length();
			float num = Math.Abs(value) / 20f;
			num -= 0.9f;
			num = MathHelper.Clamp(num, -0.9f, 0f);
			m_EngineBrakeSound.Pitch = num;
			break;
		}
		}
		m_PrevEngineState = m_EngineState;
	}

	public void StopAllEngineSounds()
	{
		Program.m_SoundManager.Stop(8);
		if (m_EngineSound != null)
		{
			m_EngineSound.Pause();
		}
		if (m_EngineBrakeSound != null)
		{
			m_EngineBrakeSound.Pause();
		}
		if (m_EngineIdleSound != null)
		{
			m_EngineIdleSound.Pause();
		}
	}

	public float LAY()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref m_GamepadState)).ThumbSticks;
		return DeadZone(((GamePadThumbSticks)(ref thumbSticks)).Left.Y);
	}

	private float DeadZone(float f)
	{
		f = ((Math.Abs(f) < 0.25f) ? 0f : ((!(f > 0f)) ? ((f + 0.25f) * 1.333f) : ((f - 0.25f) * 1.333f)));
		return f;
	}

	private float DeadZone(float f, float tol)
	{
		f = ((Math.Abs(f) < tol) ? 0f : ((!(f > 0f)) ? ((f + tol) * (1f / (1f - tol))) : ((f - tol) * (1f / (1f - tol)))));
		return f;
	}

	public bool Debounce(Buttons b)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Invalid comparison between Unknown and I4
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Invalid comparison between Unknown and I4
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Invalid comparison between Unknown and I4
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Invalid comparison between Unknown and I4
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Invalid comparison between Unknown and I4
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Invalid comparison between Unknown and I4
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		if ((int)b <= 512)
		{
			if ((int)b <= 32)
			{
				if ((int)b != 16)
				{
					if ((int)b == 32)
					{
						GamePadButtons buttons = ((GamePadState)(ref m_GamepadState)).Buttons;
						if ((int)((GamePadButtons)(ref buttons)).Back == 1)
						{
							GamePadButtons buttons2 = ((GamePadState)(ref m_OldGamepadState)).Buttons;
							if ((int)((GamePadButtons)(ref buttons2)).Back == 0)
							{
								return true;
							}
						}
					}
				}
				else
				{
					GamePadButtons buttons3 = ((GamePadState)(ref m_GamepadState)).Buttons;
					if ((int)((GamePadButtons)(ref buttons3)).Start == 1)
					{
						GamePadButtons buttons4 = ((GamePadState)(ref m_OldGamepadState)).Buttons;
						if ((int)((GamePadButtons)(ref buttons4)).Start == 0)
						{
							return true;
						}
					}
				}
			}
			else if ((int)b != 256)
			{
				if ((int)b == 512)
				{
					GamePadButtons buttons5 = ((GamePadState)(ref m_GamepadState)).Buttons;
					if ((int)((GamePadButtons)(ref buttons5)).RightShoulder == 1)
					{
						GamePadButtons buttons6 = ((GamePadState)(ref m_OldGamepadState)).Buttons;
						if ((int)((GamePadButtons)(ref buttons6)).RightShoulder == 0)
						{
							return true;
						}
					}
				}
			}
			else
			{
				GamePadButtons buttons7 = ((GamePadState)(ref m_GamepadState)).Buttons;
				if ((int)((GamePadButtons)(ref buttons7)).LeftShoulder == 1)
				{
					GamePadButtons buttons8 = ((GamePadState)(ref m_OldGamepadState)).Buttons;
					if ((int)((GamePadButtons)(ref buttons8)).LeftShoulder == 0)
					{
						return true;
					}
				}
			}
		}
		else if ((int)b <= 8192)
		{
			if ((int)b != 4096)
			{
				if ((int)b == 8192)
				{
					GamePadButtons buttons9 = ((GamePadState)(ref m_GamepadState)).Buttons;
					if ((int)((GamePadButtons)(ref buttons9)).B == 1)
					{
						GamePadButtons buttons10 = ((GamePadState)(ref m_OldGamepadState)).Buttons;
						if ((int)((GamePadButtons)(ref buttons10)).B == 0)
						{
							return true;
						}
					}
				}
			}
			else
			{
				GamePadButtons buttons11 = ((GamePadState)(ref m_GamepadState)).Buttons;
				if ((int)((GamePadButtons)(ref buttons11)).A == 1)
				{
					GamePadButtons buttons12 = ((GamePadState)(ref m_OldGamepadState)).Buttons;
					if ((int)((GamePadButtons)(ref buttons12)).A == 0)
					{
						return true;
					}
				}
			}
		}
		else if ((int)b != 16384)
		{
			if ((int)b == 32768)
			{
				GamePadButtons buttons13 = ((GamePadState)(ref m_GamepadState)).Buttons;
				if ((int)((GamePadButtons)(ref buttons13)).Y == 1)
				{
					GamePadButtons buttons14 = ((GamePadState)(ref m_OldGamepadState)).Buttons;
					if ((int)((GamePadButtons)(ref buttons14)).Y == 0)
					{
						return true;
					}
				}
			}
		}
		else
		{
			GamePadButtons buttons15 = ((GamePadState)(ref m_GamepadState)).Buttons;
			if ((int)((GamePadButtons)(ref buttons15)).X == 1)
			{
				GamePadButtons buttons16 = ((GamePadState)(ref m_OldGamepadState)).Buttons;
				if ((int)((GamePadButtons)(ref buttons16)).X == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool TakeDamage(int damage)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (m_InvulnerableTime > (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			return false;
		}
		if (Program.m_App.m_Cheats)
		{
			return false;
		}
		m_Health -= damage;
		if (m_Health < 0 && !m_bDying)
		{
			SetDying();
			m_Recover = m_MaxRecover;
			m_DeathSmokeId = CreateDeathSmoke();
			m_DeathSparksId = CreateDeathSparks(Vector2.Zero);
			if (m_Avatar != null)
			{
				m_Avatar.StartAnimation(Program.m_App.m_FlailAnimation, LoopAnimation: true);
			}
		}
		return true;
	}

	public void CheckDead()
	{
	}

	public bool DecLives()
	{
		Program.m_App.m_Lives--;
		if (Program.m_App.m_Lives <= 0)
		{
			Program.m_App.m_Lives = 5;
			Program.m_App.m_NextState = App.STATE.MAP;
			Program.m_App.m_bFadeMusic = true;
			Program.m_App.StartFade(up: false);
			return true;
		}
		return false;
	}

	public void SetDying()
	{
		m_bDying = true;
	}

	public void RemovePickups()
	{
	}

	public void UpdateScore()
	{
		bool backWheelOnGound = true;
		bool frontWheelOnGround = true;
		if (m_Ragdoll.m_bOnBike)
		{
			if (!m_bBackWheelOnGround && m_BackWheelOnFloorTime + 700f < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				backWheelOnGound = false;
			}
			if (!m_bFrontWheelOnGround && m_FrontWheelOnFloorTime + 700f < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				frontWheelOnGround = false;
			}
		}
		else
		{
			backWheelOnGound = true;
			frontWheelOnGround = true;
		}
		if (m_State == State.RACING)
		{
			UpdateAirScore(backWheelOnGound, frontWheelOnGround);
			UpdateWheelieScore(backWheelOnGound, frontWheelOnGround);
			UpdateEndoScore(backWheelOnGound, frontWheelOnGround);
			UpdateSpinScore(backWheelOnGound, frontWheelOnGround);
		}
		if (m_ScoreTally >= 100)
		{
			m_Score += 100;
			m_LevelScore += 100;
			m_ScoreTally -= 100;
		}
		else
		{
			m_Score += m_ScoreTally;
			m_LevelScore += m_ScoreTally;
			m_ScoreTally = 0;
		}
	}

	public void UpdateAirScore(bool backWheelOnGound, bool frontWheelOnGround)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (!m_Ragdoll.m_bOnBike)
		{
			m_AirTime = 0f;
			m_AirScore = 0;
			return;
		}
		if (!backWheelOnGound && !frontWheelOnGround && Math.Abs(m_ChasisBody.LinearVelocity.X) > 0.25f)
		{
			m_AirTime += (float)Program.m_App.m_GameTime.ElapsedGameTime.TotalMilliseconds;
		}
		else
		{
			if (m_AirScore > m_MaxAirScore)
			{
				m_MaxAirScore = m_AirScore;
			}
			if (m_AirTime > m_MaxAirTime)
			{
				m_MaxAirTime = m_AirTime;
			}
			m_ScoreTally += m_AirScore;
			m_AirTime = 0f;
			m_AirScore = 0;
		}
		if (m_AirTime > 500f)
		{
			m_AirScore += 100;
			m_AirDisplayScoreTime = 1f;
			m_AirDisplayScore = m_AirScore;
		}
	}

	public void UpdateEndoScore(bool backWheelOnGound, bool frontWheelOnGround)
	{
		if (!m_Ragdoll.m_bOnBike)
		{
			m_EndoTime = 0f;
			m_EndoScore = 0;
			return;
		}
		if (!backWheelOnGound && frontWheelOnGround)
		{
			m_EndoTime += (float)Program.m_App.m_GameTime.ElapsedGameTime.TotalMilliseconds;
		}
		else
		{
			if (m_EndoScore > m_MaxEndoScore)
			{
				m_MaxEndoScore = m_EndoScore;
			}
			if (m_EndoTime > m_MaxEndoTime)
			{
				m_MaxEndoTime = m_EndoTime;
			}
			m_ScoreTally += m_EndoScore;
			m_EndoTime = 0f;
			m_EndoScore = 0;
		}
		if (m_EndoTime > 500f)
		{
			m_EndoScore += 100;
			m_EndoDisplayScoreTime = 1f;
			m_EndoDisplayScore = m_EndoScore;
		}
	}

	public void UpdateWheelieScore(bool backWheelOnGound, bool frontWheelOnGround)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (!m_Ragdoll.m_bOnBike)
		{
			m_WheelieTime = 0f;
			m_WheelieScore = 0;
			return;
		}
		if (backWheelOnGound && !frontWheelOnGround && Math.Abs(m_ChasisBody.LinearVelocity.X) > 0.25f)
		{
			m_WheelieTime += (float)Program.m_App.m_GameTime.ElapsedGameTime.TotalMilliseconds;
		}
		else
		{
			if (m_WheelieScore > m_MaxWheelieScore)
			{
				m_MaxWheelieScore = m_WheelieScore;
			}
			if (m_WheelieTime > m_MaxWheelieTime)
			{
				m_MaxWheelieTime = m_WheelieTime;
			}
			m_ScoreTally += m_WheelieScore;
			m_WheelieTime = 0f;
			m_WheelieScore = 0;
		}
		if (m_WheelieTime > 500f)
		{
			m_WheelieScore += 100;
			m_WheelieDisplayScoreTime = 1f;
			m_WheelieDisplayScore = m_WheelieScore;
		}
	}

	private void UpdateSpinScore(bool backWheelOnGound, bool frontWheelOnGround)
	{
		if (!m_Ragdoll.m_bOnBike)
		{
			m_SpinRevolutions = 0;
			m_SpinScore = 0;
		}
		else if (!backWheelOnGound && !frontWheelOnGround)
		{
			if (m_ChasisBody.Rotation > 5.7831855f || m_ChasisBody.Rotation < -5.7831855f)
			{
				if (m_ChasisBody.Rotation > 0f)
				{
					m_BackFlip = true;
				}
				else
				{
					m_BackFlip = false;
				}
				m_ChasisBody.Rotation = Fn.NormRot(m_ChasisBody.Rotation);
				m_SpinRevolutions++;
				m_SpinScore += 10000;
				m_SpinDisplayRevolutions = m_SpinRevolutions;
			}
			if (m_SpinRevolutions > 0)
			{
				m_SpinDisplayScoreTime = 2f;
			}
		}
		else
		{
			m_SpinRevolutions = 0;
			m_ScoreTally += m_SpinScore;
			m_SpinScore = 0;
		}
	}

	public bool GivePickup(int ItemId)
	{
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		if (m_bDying)
		{
			return false;
		}
		if (!m_Ragdoll.m_bOnBike)
		{
			return false;
		}
		if (m_State != State.RACING)
		{
			return false;
		}
		switch (Program.m_ItemManager.m_Item[ItemId].m_Type)
		{
		case 21:
		case 22:
		case 23:
			if (m_State != State.RACING)
			{
				return false;
			}
			Program.m_ItemManager.m_Item[ItemId].CollectFlag();
			Program.m_App.m_CollectFlagTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 550f;
			Program.m_App.m_CollectFlagPosition.X = Program.m_ItemManager.m_Item[ItemId].m_ScreenPos.X;
			Program.m_App.m_CollectFlagPosition.Y = Program.m_ItemManager.m_Item[ItemId].m_ScreenPos.Y;
			Program.m_App.m_CollectFlagScale = 1f;
			Program.m_ItemManager.Delete(ItemId);
			Program.m_SoundManager.Play(24);
			break;
		case 12:
		case 38:
		case 58:
			if (m_Ragdoll.m_bOnBike)
			{
				Crash();
				Body chasisBody = m_ChasisBody;
				chasisBody.LinearVelocity += new Vector2(0f, 20f);
				Body body = m_Ragdoll.Body;
				body.LinearVelocity += new Vector2(0f, 50f);
				Program.m_SoundManager.Play(30);
			}
			break;
		case 62:
			Program.m_LoadSaveManager.SaveCheckpoint(Program.m_ItemManager.m_Item[ItemId].m_Position, bShowMessage: true);
			break;
		}
		return false;
	}

	public Vector2 ClosestPointOnLine(Vector2 l1, Vector2 l2, Vector2 pt)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = l2 - l1;
		Vector2 val2 = pt - l1;
		if (Vector2.Dot(val2, val) < 0f)
		{
			return l1;
		}
		Vector2 val3 = default(Vector2);
		((Vector2)(ref val3))._002Ector(val.X, val.Y);
		((Vector2)(ref val3)).Normalize();
		float num = Vector2.Dot(val2, val3);
		if (num * num > Vector2.Dot(val, val))
		{
			return l2;
		}
		return l1 + val3 * num;
	}

	public int CreateSmokeTrail()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, -0.01f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 2000f;
		def.m_fRate = 60f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = Program.m_App.m_ToonSmoke1Texture;
		def.m_NumFrames = def.m_Image.Width / def.m_Image.Height;
		def.m_NumParticles = 10;
		def.m_Origin = new Vector2((float)def.m_Image.Height * 0.5f, (float)def.m_Image.Height * 0.5f);
		def.m_Position = m_Position + new Vector2(0f, 10f);
		def.m_Velocity = new Vector2(-3f, 0f);
		def.m_VelocityVar = new Vector2(1f, 0.5f);
		def.m_Colour = new Color(0.9f, 0.9f, 0.9f, 1f);
		def.m_fRotation = 0.01f;
		def.m_Track = this;
		def.m_Flags = 4u;
		def.m_Offset = new Vector2(-70f, 0f);
		def.m_fScale = 0.5f;
		def.m_Font = null;
		def.m_String = null;
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		return Program.m_ParticleManager.Create(def);
	}

	public void CreateWheelTrail()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, -0.01f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 1000f;
		def.m_fRate = 50f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = Program.m_App.m_ToonSmokeDarkGreyTexture;
		def.m_NumFrames = def.m_Image.Width / def.m_Image.Height;
		def.m_NumParticles = 10;
		def.m_Origin = new Vector2((float)def.m_Image.Height * 0.5f, (float)def.m_Image.Height * 0.5f);
		def.m_Position = m_Position + new Vector2(0f, 0f);
		def.m_Velocity = new Vector2(-3f, 0f);
		def.m_VelocityVar = new Vector2(1f, 1f);
		def.m_Colour = new Color(1f, 1f, 1f, 1f);
		def.m_fRotation = 0.01f;
		def.m_Track = this;
		def.m_Flags = 4u;
		def.m_Offset = new Vector2(10f, 60f);
		def.m_fScale = 0.5f;
		def.m_Font = null;
		def.m_String = null;
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		m_WheelTrailId = Program.m_ParticleManager.Create(def);
	}

	public void CreateSparkTrail()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, 0.2f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 500f;
		def.m_fRate = 5f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = null;
		def.m_NumFrames = 0;
		def.m_NumParticles = 20;
		def.m_Origin = new Vector2(0f, 0f);
		def.m_Position = new Vector2(m_ScreenPos.X, m_ScreenPos.Y);
		def.m_Velocity = new Vector2(0f, -4f);
		def.m_VelocityVar = new Vector2(4.5f, 4.5f);
		def.m_Colour = new Color(1f, 1f, 1f, 0.5f);
		def.m_fRotation = 0.01f;
		def.m_Track = this;
		def.m_Flags = 5u;
		def.m_Offset = new Vector2(25f, 65f);
		def.m_fScale = 1f;
		def.m_Font = null;
		def.m_String = null;
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		m_SparkTrailId = Program.m_ParticleManager.Create(def);
	}

	public void CreateScrapeEmitter()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, 0f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 1000f;
		def.m_fRate = 0f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = null;
		def.m_NumFrames = 0;
		def.m_NumParticles = 2;
		def.m_Origin = new Vector2(0f, 0f);
		Vector3 zero = Vector3.Zero;
		((Vector3)(ref zero))._002Ector(m_Position.X, m_Position.Y, 0f);
		Vector3 val = Program.m_CameraManager3D.WorldToScreen(zero);
		def.m_Position = new Vector2(val.X, val.Y);
		def.m_Velocity = new Vector2(-1f, 0f);
		def.m_VelocityVar = new Vector2(0.2f, 0.2f);
		def.m_Colour = new Color(byte.MaxValue, byte.MaxValue, (byte)0, byte.MaxValue);
		def.m_fRotation = 0f;
		def.m_Track = null;
		def.m_Flags = 24u;
		def.m_Offset.X = Program.m_App.m_Rand.Next(-10, 10);
		def.m_Offset.Y = Program.m_App.m_Rand.Next(-100, -60);
		def.m_fScale = 1f;
		def.m_Font = Program.m_App.m_GameText;
		def.m_String = Program.m_App.GetText(App.TEXTID.SCRAPE);
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		Program.m_ParticleManager.Create(def);
	}

	public void CreateTakeOffEmitter()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, 0f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 1000f;
		def.m_fRate = 500f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = Program.m_App.m_ArrowTexture;
		def.m_NumFrames = 1;
		def.m_NumParticles = 3;
		def.m_Origin = new Vector2((float)def.m_Image.Height * 0.5f, (float)def.m_Image.Height * 0.5f);
		Vector3 zero = Vector3.Zero;
		((Vector3)(ref zero))._002Ector(m_Position.X, m_Position.Y, 0f);
		Vector3 val = Program.m_CameraManager3D.WorldToScreen(zero);
		def.m_Position = new Vector2(val.X, val.Y);
		def.m_Velocity = new Vector2(0f, -5f);
		def.m_VelocityVar = new Vector2(0f, 0f);
		def.m_Colour = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		def.m_fRotation = 0f;
		def.m_Track = null;
		def.m_Flags = 0u;
		def.m_Offset.X = 200f;
		def.m_Offset.Y = 0f;
		def.m_fScale = 1f;
		def.m_Font = null;
		def.m_String = null;
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		Program.m_ParticleManager.Create(def);
	}

	public int CreateDeathSmoke()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, -0.01f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 1000f;
		def.m_fRate = 30f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = Program.m_App.m_ToonSmokeDarkGreyTexture;
		def.m_NumFrames = def.m_Image.Width / def.m_Image.Height;
		def.m_NumParticles = 10;
		def.m_Origin = new Vector2((float)def.m_Image.Height * 0.5f, (float)def.m_Image.Height * 0.5f);
		def.m_Position = m_Position + new Vector2(0f, 10f);
		def.m_Velocity = new Vector2(-6f, 0f);
		def.m_VelocityVar = new Vector2(1f, 0.5f);
		def.m_Colour = new Color(1f, 1f, 1f, 1f);
		def.m_fRotation = 0.01f;
		def.m_Track = this;
		def.m_Flags = 4u;
		def.m_Offset = new Vector2(0f, 0f);
		def.m_fScale = 0.6f;
		def.m_Font = null;
		def.m_String = null;
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		return Program.m_ParticleManager.Create(def);
	}

	public int CreateDeathSparks(Vector2 offset)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, 0.2f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 500f;
		def.m_fRate = 17f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = null;
		def.m_NumFrames = 0;
		def.m_NumParticles = 10;
		def.m_Origin = new Vector2(0f, 0f);
		def.m_Position = m_Position + new Vector2(0f, 10f);
		def.m_Velocity = new Vector2(-6f, -6f);
		def.m_VelocityVar = new Vector2(3.5f, 3.5f);
		def.m_Colour = new Color(1f, 1f, 1f, 1f);
		def.m_fRotation = 0.01f;
		def.m_Track = this;
		def.m_Flags = 5u;
		def.m_Offset = offset;
		def.m_fScale = 1f;
		def.m_Font = null;
		def.m_String = null;
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		return Program.m_ParticleManager.Create(def);
	}

	public int CreatePlayerExplosion()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		Program.m_CameraManager3D.Shake(250f, 0.2f);
		Program.m_App.m_RumbleFrames = 6;
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, 0.02f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 1500f;
		def.m_fRate = 0f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = Program.m_App.m_ToonSmokeYellowTexture;
		def.m_NumFrames = def.m_Image.Width / def.m_Image.Height;
		def.m_NumParticles = 15;
		def.m_Origin = new Vector2((float)def.m_Image.Height * 0.5f, (float)def.m_Image.Height * 0.5f);
		def.m_Position = new Vector2(m_ScreenPos.X, m_ScreenPos.Y);
		def.m_Velocity = new Vector2(0f, -1f);
		def.m_VelocityVar = new Vector2(1.5f, 1f);
		def.m_Colour = new Color(1f, 1f, 1f, 1f);
		def.m_fRotation = 0.2f;
		def.m_Track = null;
		def.m_Flags = 528u;
		def.m_Offset = new Vector2(-30f, 0f);
		def.m_fScale = 1f;
		def.m_Font = null;
		def.m_String = null;
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		return Program.m_ParticleManager.Create(def);
	}

	public void CreateLandingBonusEmitter()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = default(Vector2);
		((Vector2)(ref position))._002Ector(550f, 600f);
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, 0.2f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 5000f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = null;
		def.m_fRate = 10f;
		def.m_NumFrames = 0;
		def.m_NumParticles = 10;
		def.m_Origin = new Vector2(0f, 0f);
		def.m_Position = position;
		def.m_Velocity = new Vector2(-1f, -13f);
		def.m_VelocityVar = new Vector2(5f, 2f);
		def.m_Colour = Color.White;
		def.m_fRotation = 0f;
		def.m_Track = null;
		def.m_Flags = 8u;
		def.m_Offset.X = Program.m_App.m_Rand.Next(-10, 10);
		def.m_Offset.Y = Program.m_App.m_Rand.Next(-40, 0);
		def.m_fScale = 1f;
		def.m_Font = Program.m_App.m_GameText;
		def.m_String = Program.m_App.GetText(App.TEXTID.LANDING_BONUS);
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		Program.m_ParticleManager.Create(def);
	}

	public void Reset()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		m_Position.X = 0f;
		m_Position.Y = 0f;
		m_ZDistance = 0f - (float)m_Id * 1f;
		m_Velocity = Vector2.Zero;
		m_Roll = (float)Program.m_App.m_Rand.NextDouble() * 3f - 1.5f;
		m_bDoCollision = true;
		if (Program.m_App.m_GameTime != null)
		{
			m_WaitingAtTowerTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 3000f;
		}
		else
		{
			m_WaitingAtTowerTime = 6000f;
		}
		m_TaxiingTime = 0f;
		m_bCanBeOnGround = true;
		RemovePickups();
		if (m_SmokeTrailId != -1)
		{
			Program.m_ParticleManager.Delete(m_SmokeTrailId);
			m_SmokeTrailId = -1;
		}
		if (m_SparkTrailId != -1)
		{
			Program.m_ParticleManager.Delete(m_SparkTrailId);
			m_SparkTrailId = -1;
		}
		if (m_WheelTrailId != -1)
		{
			Program.m_ParticleManager.Delete(m_WheelTrailId);
			m_WheelTrailId = -1;
		}
		if (m_EngineSound != null)
		{
			m_EngineSound.Stop();
			m_EngineSound = null;
		}
		if (m_DiveSound != null)
		{
			m_DiveSound.Stop();
			m_DiveSound = null;
		}
		m_bDying = false;
		m_Health = 100;
		m_MaxRecover = 6;
		m_Recover = m_MaxRecover;
		m_ScrapeCounter = 0;
		if (m_Avatar != null)
		{
			m_Avatar.StartAnimation(Program.m_App.m_IdleGripAnimation, LoopAnimation: false);
		}
		m_bShowAButton = false;
		m_bShowBButton = false;
		m_AirScore = 0;
		m_AirTime = 0f;
		m_MaxAirScore = 0;
		m_MaxAirTime = 0f;
		m_WheelieScore = 0;
		m_WheelieTime = 0f;
		m_MaxWheelieScore = 0;
		m_MaxWheelieTime = 0f;
		m_EndoScore = 0;
		m_EndoTime = 0f;
		m_MaxEndoScore = 0;
		m_MaxEndoTime = 0f;
		Program.m_App.m_TrackTime = 0f;
		m_Score = 0;
		m_ScoreTally = 0;
		Program.m_App.m_TextScale = 3f;
	}

	private void UpdateEditor()
	{
		if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)37))
		{
			Program.m_App.m_LevelPos -= 0.5f;
		}
		if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)39))
		{
			Program.m_App.m_LevelPos += 0.5f;
		}
		if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)38))
		{
			ref Vector3 cameraPositionTarget = ref Program.m_CameraManager3D.m_CameraPositionTarget;
			cameraPositionTarget.Y += 0.1f;
			ref Vector3 cameraLookAtTarget = ref Program.m_CameraManager3D.m_CameraLookAtTarget;
			cameraLookAtTarget.Y += 0.1f;
		}
		if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)40))
		{
			ref Vector3 cameraPositionTarget2 = ref Program.m_CameraManager3D.m_CameraPositionTarget;
			cameraPositionTarget2.Y -= 0.1f;
			ref Vector3 cameraLookAtTarget2 = ref Program.m_CameraManager3D.m_CameraLookAtTarget;
			cameraLookAtTarget2.Y -= 0.1f;
		}
		m_Velocity.X = (m_Velocity.Y = 0f);
	}
}
