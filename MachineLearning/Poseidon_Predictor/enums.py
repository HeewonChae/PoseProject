from enum import Enum


class MatchResultType(Enum):
    Win = 0
    Draw = 1
    Lose = 2


class CampType(Enum):
    Home = 0
    Away = 1
    Any = 2


class LeagueType(Enum):
    Cup = 0
    League = 1


class SportsType(Enum):
    Football = 1
