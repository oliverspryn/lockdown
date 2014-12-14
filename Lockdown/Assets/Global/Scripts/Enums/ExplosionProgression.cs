/// <summary>
/// Define how the explosions from the <c>Destroyer</c> will take
/// place, whether it starts from one side and goes to another, or
/// if the explosions will take place everywhere at once.
/// </summary>
public enum ExplosionProgression {
	Everywhere = 0,
	BackToFront,
	BottomToTop,
	FrontToBack,
	LeftToRight,
	RightToLeft,
	TopToBottom
}