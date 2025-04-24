# Changelog
## [0.4.1]
### Added
- C8 & C4 texture support, Including the covidted ARGB8888 (AKA 2 IA8 pallets stiched together)
- Allow export of said formats to png via the now working Preview window image.

## [0.4.0]
### Added
- Preview window for textures

### Changed
- Inital Implenation of cleaner VGT reading
## [0.3.1]
### Added
- Export textures as PNG (unfinished/broken). Exporting RGB textures works but uses the wrong color format.
### Changed
- Names for texture formats now match GC texture types. DXT1 is now labelled as CMPR.
### Fixed 
- Unhandled exception when replacing files.
- Soft lock that occurred when opening a different OLK file.

## [0.3.0-b]
### Added 
- Can manually set the number of mipmap textures to import/export. (Todo: figure out how to get this automatically)
- Texture data/header offsets are now displayed.

## [0.3.0-a]
### Added 
- mmg extraction. Not sure how to get size and current method doesn't work for some files.

## [0.2.0]
### Added
- .vmp file detection. (Name may change)
- .mot file detection. (Name may change)
- .mot entry viewer. Can export byte code to a .txt file.
- File replacement. Only works with files in the root olk opened at the moment.

### Changed
- Files can now be extracted from .pkg containers.
- Improved getting the correct resolution when selecting a VGT texture.

## [0.1.0]
### Added
- Initial Release.
- File extraction. Doesn't work with files in .pkg containers.
- VGT import/export. Default image dimensions may be wrong.